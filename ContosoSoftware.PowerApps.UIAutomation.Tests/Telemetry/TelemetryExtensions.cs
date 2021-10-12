using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;
using Microsoft.Dynamics365.UIAutomation.Browser.Extensions;
using Newtonsoft.Json;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoSoftware.PowerApps.UIAutomation.Tests.Telemetry
{
    public class TelemetryExtensions : Element
    {
        private readonly WebClient _client;
        private ILogger _logger;
        /// <summary>
        /// Enables Performance Center for UCI 
        /// </summary>
        public void EnablePerformanceCenter()
        {
            //_client.Browser.Driver.Navigate().GoToUrl($"{_client.Browser.Driver.Url}&perf=true");
            _client.Browser.Driver.WaitForPageToLoad();
            _client.Browser.Driver.WaitForTransaction();
        }

        public TelemetryExtensions(WebClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public Dictionary<string, string> ReturnMetadata()
        {
            return new Dictionary<string, string>();
        }

        #region Private
        public enum BrowserEventType
        {
            Resource,
            Navigation,
            Server
        }

        /// <summary>
        /// Track Command Events will track the Command Results from the API executions to Application Insights with the Instrumentation Key provided in the Options. 
        /// </summary>
        /// <param name="additionalProperties">The additional properties you want to track in telemetry. These values will show up in the customDimensions of the customEvents</param>
        /// <param name="additionalMetrics">The additional metricsyou want to track in telemetry. These values will show up in the customMeasurements of the customEvents</param>
        public void TrackCommandEvents(Dictionary<string, string> additionalProperties = null, Dictionary<string, double> additionalMetrics = null)
        {
            _client.CommandResults.ForEach(x =>
            {
                var properties = new Dictionary<string, string>();
                var metrics = new Dictionary<string, double>();

                if (additionalMetrics != null && additionalMetrics.Count > 0)
                    metrics = metrics.Merge(additionalMetrics);

                if (additionalProperties != null && additionalProperties.Count > 0)
                    properties = properties.Merge(additionalProperties);

                properties.Add("StartTime", x.StartTime.Value.ToLongDateString());
                properties.Add("EndTime", x.StopTime.Value.ToLongDateString());

                metrics.Add("ThinkTime", x.ThinkTime);
                metrics.Add("TransitionTime", x.TransitionTime);
                metrics.Add("ExecutionTime", x.ExecutionTime);
                metrics.Add("ExecutionAttempts", x.ExecutionAttempts);

                TrackEvents(x.CommandName, properties, metrics);
            });
        }

        private List<EventTelemetry> CollectTimelineEvents(List<TimelineEvent> timelineEvents)
        {
            Debug.WriteLine("Entered into CollectTimelineEvents");
            List<EventTelemetry> rtnObject = new List<EventTelemetry>();
            foreach (TimelineEvent customControlEvent in timelineEvents)
            {
                if (customControlEvent.parameters.ContainsKey("manifestControlName"))
                {
                    if (customControlEvent.parameters.Where(x => x.Key == "manifestControlName").FirstOrDefault().Value.ToString().Contains("MscrmControls.FieldControls"))
                    {
                        EventTelemetry customControlTimelineEventEventTelemetry = new EventTelemetry();
                        customControlTimelineEventEventTelemetry.Name = customControlEvent.name;
                        customControlTimelineEventEventTelemetry.Properties.Add("zone", customControlEvent.zone);
                        customControlTimelineEventEventTelemetry.Metrics.Add("time", customControlEvent.time.GetValueOrDefault(0));
                        customControlTimelineEventEventTelemetry.Metrics.Add("timeEnd", customControlEvent.timeEnd.GetValueOrDefault(0));
                        try
                        {
                            foreach (KeyValuePair<string, object> keyValuePairs in customControlEvent.parameters)
                            {
                                customControlTimelineEventEventTelemetry.Properties.Add(keyValuePairs.Key, keyValuePairs.Value.ToString());
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        rtnObject.Add(customControlTimelineEventEventTelemetry);
                        string property = customControlEvent.parameters.Where(x => x.Key == "manifestControlName").FirstOrDefault().Value.ToString();
                    }
                }

            }
            Debug.WriteLine("Exited into CollectTimelineEvents");
            return rtnObject;
        }



        /// <summary>
        /// Tracks the performance center telemetry events.
        /// </summary>
        /// <param name="additionalProperties">The additional properties you want to track in telemetry. These values will show up in the customDimensions of the customEvents</param>
        /// <param name="additionalMetrics">The additional metricsyou want to track in telemetry. These values will show up in the customMeasurements of the customEvents</param>
        /// <exception cref="System.InvalidOperationException">UCI Performance Mode is not enabled.  Please enable performance mode in the Options before tracking performance telemetry.</exception>
        public void TrackPerformanceEvents(Dictionary<string, string> additionalProperties = null, Dictionary<string, double> additionalMetrics = null)
        {
            if (!_client.Browser.Options.UCIPerformanceMode) throw new InvalidOperationException("UCI Performance Mode is not enabled.  Please enable performance mode in the Options before tracking performance telemetry.");
            ShowHidePerformanceWidget();
            _client.Browser.Options.AppInsightsKey = "f3112d0e-9637-4ef9-acb9-23f5fdd4fda0";
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient
            {
                InstrumentationKey = _client.Browser.Options.AppInsightsKey,
            };
            string generatedOperationId = Guid.NewGuid().ToString();
            telemetry.Context.Operation.Id = generatedOperationId;
            telemetry.Context.Operation.Name = "Performance Center Markers";
            telemetry.Context.Session.Id = _client.ClientSessionId.ToString();

            Dictionary<string, string> metadata = GetMetadataMarkers();
            Dictionary<string, double> markers = GetPerformanceMarkers("TimelineWall_KPI_Load");
            //Dictionary<string, double> markers = GetPerformanceMarkers("SaveForm");
            #region Timeline Events
            Dictionary<string, List<TimelineEvent>> timeline = GetTimelineEvents();
            Dictionary<string, string> timelineRaw = GetTimelineEventsRaw();
            Dictionary<string, string> timelineToReport = new Dictionary<string, string>();

            List<MetricTelemetry> timelineEventMetrics = new List<MetricTelemetry>();

            List<TimelineEvent> customControlsFramework = new List<TimelineEvent>();
            List<TimelineEvent> browserResourceTimings = new List<TimelineEvent>();

            customControlsFramework = timeline["timelineEvents"].Where(x => x.zone == "CustomControlsFramework").ToList();
            browserResourceTimings = timeline["timelineEvents"].Where(x => x.zone == "BrowserResourceTimings").ToList();

            //CustomControls
            List<EventTelemetry> customControlsTimelineEventEvents = CollectTimelineEvents(timeline["timelineEvents"].Where(x => x.zone == "CustomControls").ToList()); //Collect CustomControl timeline events
            //Send TimelineEvents
            //TrackEvents(customControlsTimelineEventEvents);

            //Core.KPI
            List<EventTelemetry> coreKpiTimelineEvents = CollectTimelineEvents(timeline["timelineEvents"].Where(x => x.zone == "Core.KPI").ToList()); //Collect CustomControl timeline events
            //Send TimelineEvents
            //TrackEvents(coreKpiTimelineEvents);


            #endregion
            #region KPI Markers
            foreach (KeyValuePair<string, double> marker in markers)
            {

            }
            #endregion

            #region unused
            //foreach (TimelineEvent timelineevent in timeline["timelineEvents"])
            //{

            //        switch (timelineevent.name)
            //        {
            //            case "SaveForm":
            //            eventTelemetry = new EventTelemetry();
            //            eventTelemetry.Properties.Add(timelineevent.name, timelineevent.name);
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            timelineEventEvents.Add(eventTelemetry);

            //            metricTelemetry = new MetricTelemetry();
            //            metricTelemetry.Name = timelineevent.name;
            //            metricTelemetry.Min = (double?)timelineevent.time;
            //            metricTelemetry.Max = (double?)timelineevent.timeEnd;
            //            metricTelemetry.MetricNamespace = timelineevent.zone;
            //            timelineEventMetrics.Add(metricTelemetry);
            //            timelineToReport.Add(timelineevent.name, timelineevent.name);
            //                timelineToReport.Add(timelineevent.name + ".time", timelineevent.time.ToString());
            //                timelineToReport.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.ToString() ?? "");

            //            markers.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            markers.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            break;
            //            case "WorkStart":
            //            eventTelemetry = new EventTelemetry();
            //            eventTelemetry.Properties.Add(timelineevent.name, timelineevent.name);
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            timelineEventEvents.Add(eventTelemetry);

            //            metricTelemetry = new MetricTelemetry();
            //            metricTelemetry.Name = timelineevent.name;
            //            metricTelemetry.Min = (double?)timelineevent.time;
            //            metricTelemetry.Max = (double?)timelineevent.timeEnd;
            //            metricTelemetry.MetricNamespace = timelineevent.zone;
            //            timelineEventMetrics.Add(metricTelemetry);
            //            timelineToReport.Add(timelineevent.name, timelineevent.name);
            //                timelineToReport.Add(timelineevent.name + ".time", timelineevent.time.ToString());
            //                timelineToReport.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.ToString() ?? "");


            //            markers.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            markers.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            break;
            //            case "WorkEnd":
            //            eventTelemetry = new EventTelemetry();
            //            eventTelemetry.Properties.Add(timelineevent.name, timelineevent.name);
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            eventTelemetry.Metrics.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            timelineEventEvents.Add(eventTelemetry);

            //            metricTelemetry = new MetricTelemetry();
            //            metricTelemetry.Name = timelineevent.name;
            //            metricTelemetry.Min = (double?)timelineevent.time;
            //            metricTelemetry.Max = (double?)timelineevent.timeEnd;
            //            metricTelemetry.MetricNamespace = timelineevent.zone;
            //            timelineEventMetrics.Add(metricTelemetry);
            //            timelineToReport.Add(timelineevent.name, timelineevent.name);
            //                timelineToReport.Add(timelineevent.name + ".time", timelineevent.time.ToString());
            //                timelineToReport.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.ToString() ?? "");

            //            markers.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            markers.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            break;//"BrowserResourceTimings"
            //            case "XMLHttpRequest":
            //            eventTelemetry = new EventTelemetry();

            //            eventTelemetry.Properties.Add(timelineevent.parameters["name"].ToString(), timelineevent.name);
            //            eventTelemetry.Metrics.Add(timelineevent.parameters["name"].ToString() + ".time", timelineevent.time.GetValueOrDefault(0));
            //            eventTelemetry.Metrics.Add(timelineevent.parameters["name"].ToString() + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            timelineEventEvents.Add(eventTelemetry);
            //            //PerformanceCounterTelemetry performanceCounterTelemetry = new PerformanceCounterTelemetry();
            //            //performanceCounterTelemetry.CategoryName = timelineevent.zone;
            //            //performanceCounterTelemetry.CounterName = timelineevent.name;
            //            //performanceCounterTelemetry.Value = timelineevent.timeEnd.GetValueOrDefault(0) - timelineevent.time.GetValueOrDefault(0);

            //            metricTelemetry = new MetricTelemetry();
            //            metricTelemetry.Name = timelineevent.name;
            //            metricTelemetry.Min = (double?)timelineevent.time;
            //            metricTelemetry.Max = (double?)timelineevent.timeEnd;
            //            metricTelemetry.MetricNamespace = timelineevent.zone;
            //            timelineEventMetrics.Add(metricTelemetry);
            //            //timelineToReport.Add(timelineevent.parameters["name"].ToString(), timelineevent.name);
            //            //timelineToReport.Add(timelineevent.parameters["name"].ToString() + ".time", timelineevent.time.ToString());
            //            //timelineToReport.Add(timelineevent.parameters["name"].ToString() + ".timeEnd", timelineevent.timeEnd.ToString() ?? "");

            //            markers.Add(timelineevent.name + ".time", timelineevent.time.GetValueOrDefault(0));
            //            markers.Add(timelineevent.name + ".timeEnd", timelineevent.timeEnd.GetValueOrDefault(0));
            //            break;//"BrowserResourceTimings"
            //            default: break;
            //        }


            //}
            #endregion

            ShowHidePerformanceWidget();
            metadata.Add("ClientSessionId", _client.ClientSessionId.ToString());

            if (additionalMetrics != null && additionalMetrics.Count > 0)
                markers = markers.Merge(additionalMetrics);

            if (additionalProperties != null && additionalProperties.Count > 0)
                metadata = metadata.Merge(additionalProperties);

            if (timelineRaw != null && timelineRaw.Count > 0)
                metadata = metadata.Merge(timelineToReport);

            //TrackMetrics("Performance Center Markers", metadata, timelineEventMetrics);
            //OOTB
            //TrackEvents("Performance Markers", metadata, markers);


            telemetry = null;
        }

        //internal void TrackEvents(List<EventTelemetry> events)
        //{
        //    //if (string.IsNullOrEmpty(_client.Browser.Options.AppInsightsKey)) throw new InvalidOperationException("The Application Insights key was not specified.  Please specify an Instrumentation key in the Browser Options.");
        //    //var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _client.Browser.Options.AppInsightsKey };
        //    foreach (var @event in events)
        //    {
        //        //telemetry.TrackEvent(@event);
        //        _logger.Log()
        //    }

        //}
        //internal void TrackMetrics(string eventName, Dictionary<string, string> properties, List<MetricTelemetry> metrics)
        //{
        //    //properties.Add("ClientSessionId", _client.ClientSessionId.ToString());
        //    foreach (var metric in metrics)
        //    {
        //        telemetry.TrackMetric(metric);
        //    }

        //    //telemetry.TrackEvent(eventName, properties, metrics);
        //    telemetry.Flush();
        //}

        /// <summary>
        /// Tracks the browser window.performance events.
        /// </summary>
        /// <param name="type">The type of window.performance timings you want to track.</param>
        /// <param name="additionalProperties">The additional properties you want to track in telemetry. These values will show up in the customDimensions of the customEvents</param>
        /// <param name="clearTimings">if set to <c>true</c> clears the resource timings.</param>
        public void TrackBrowserEvents(BrowserEventType type, Dictionary<string, string> additionalProperties = null, bool clearTimings = false)
        {
            var properties = GetBrowserTimings(type);

            if (additionalProperties != null) properties = properties.Merge(additionalProperties);

            if (properties.Count > 0) TrackEvents(type.ToString(), properties, null);
        }

        internal void TrackEvents(string eventName, Dictionary<string, string> properties, Dictionary<string, double> metrics)
        {
            _client.Browser.Options.AppInsightsKey = "7b18b7f7-3daf-4951-abba-8372cf9b21a9";
            if (string.IsNullOrEmpty(_client.Browser.Options.AppInsightsKey)) throw new InvalidOperationException("The Application Insights key was not specified.  Please specify an Instrumentation key in the Browser Options.");
            properties.Add("ClientSessionId", _client.ClientSessionId.ToString());

            //var telemetry = new Microsoft.ApplicationInsights.TelemetryClient { InstrumentationKey = _client.Browser.Options.AppInsightsKey };
            //telemetry.TrackEvent(eventName, properties, metrics);
            //telemetry.Flush();

            //telemetry = null;
        }

        public Dictionary<string, double> GetPerformanceMarkers(string page)
        {

            SelectPerformanceWidgetPage(page);

            var jsonResults = _client.Browser.Driver.ExecuteScript("return JSON.stringify(UCPerformanceTimeline.getKeyPerformanceIndicators())").ToString();
            var list = JsonConvert.DeserializeObject<List<KeyPerformanceIndicator>>(jsonResults);

            Dictionary<string, double> dict = list.ToDictionary(x => x.Name, x => x.Value);

            return dict;
        }

        internal Dictionary<string, string> GetMetadataMarkers()
        {
            Dictionary<string, object> jsonResults = (Dictionary<string, object>)_client.Browser.Driver.ExecuteScript("return UCPerformanceTimeline.getMetadata()");

            return jsonResults.ToDictionary(x => x.Key, x => x.Value.ToString());
        }
        public class TimelineEvent
        {
            public string name { get; set; }
            public double? time { get; set; }
            public double? timeEnd { get; set; }
            public string zone { get; set; }
            public Dictionary<string, object> parameters { get; set; }
        }
        internal Dictionary<string, List<TimelineEvent>> GetTimelineEvents()
        {
            var entries = (IEnumerable<object>)_client.Browser.Driver.ExecuteScript("return UCPerformanceTimeline.getEvents()");
            var jsonEntries = _client.Browser.Driver.ExecuteScript("return JSON.stringify(UCPerformanceTimeline.getEvents())").ToString();
            var results = new Dictionary<string, List<TimelineEvent>>();
            var list = JsonConvert.DeserializeObject<List<TimelineEvent>>(jsonEntries);

            results.Add("timelineEvents", list);

            //foreach (var entry in entries)
            //{
            //    var dict = (Dictionary<string, object>)entry;
            //    var todict = dict.ToDictionary(x => x.Key, x => x.Value);
            //    results = results.Merge(todict);
            //}

            return results;
        }
        internal Dictionary<string, string> GetTimelineEventsRaw()
        {
            var entries = (IEnumerable<object>)_client.Browser.Driver.ExecuteScript("return UCPerformanceTimeline.getEvents()");
            var jsonEntries = _client.Browser.Driver.ExecuteScript("return JSON.stringify(UCPerformanceTimeline.getEvents())").ToString();
            var results = new Dictionary<string, string>();
            var list = JsonConvert.DeserializeObject<List<TimelineEvent>>(jsonEntries);

            results.Add("timelineEvents", jsonEntries);

            //foreach (var entry in entries)
            //{
            //    var dict = (Dictionary<string, object>)entry;
            //    var todict = dict.ToDictionary(x => x.Key, x => x.Value);
            //    results = results.Merge(todict);
            //}

            return results;
        }
        internal Dictionary<string, string> GetBrowserTimings(BrowserEventType type, bool clearTimings = false)
        {
            var entries = (IEnumerable<object>)_client.Browser.Driver.ExecuteScript("return window.performance.getEntriesByType('" + type.ToString("g").ToLowerString() + "')");

            var results = new Dictionary<string, string>();

            foreach (var entry in entries)
            {
                var dict = (Dictionary<string, object>)entry;
                var todict = dict.ToDictionary(x => x.Key, x => x.Value.ToString());
                results = results.Merge(todict);
            }

            if (clearTimings) ClearResourceTimings();

            return results;
        }

        public void ShowHidePerformanceWidget()
        {
            _client.Browser.Driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.PerformanceWidget.Container]));
        }
        internal void SelectPerformanceWidgetPage(string page)
        {
            _client.Browser.Driver.ClickWhenAvailable(By.XPath(AppElements.Xpath[AppReference.PerformanceWidget.Page].Replace("[NAME]", page)));
        }

        internal void ClearResourceTimings()
        {
            _client.Browser.Driver.ExecuteScript("return window.performance.clearResourceTimings();");
        }
        #endregion


    }
}
