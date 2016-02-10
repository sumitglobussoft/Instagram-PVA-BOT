﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;

namespace BaseLib
{
    public class Events
    {
        
        public event EventHandler handleParamsEvent;

        /// <summary>
        /// Fires the event taking "EventsArgs" instance as parameter
        /// Just call this method where you want to fire the event
        /// </summary>
        /// <param name="e"></param>
        public void RaiseParamsEvent(EventsArgs e)
        {
            if (handleParamsEvent != null)
            {
                handleParamsEvent(this, e); //Fires the event
            }
        }

        public event EventHandler processCompletedEvent;

        EventHandler processCompletedEventHandlerMethod;

        public void SubscribeProcessCompletedHandler(EventHandler eventHandlerMethod)
        {
            this.processCompletedEventHandlerMethod = eventHandlerMethod;
            this.processCompletedEvent += new EventHandler(processCompletedEventHandlerMethod);
        }
        public void UnSubscribeProcessCompletedHandler()
        {
            this.processCompletedEvent -= new EventHandler(processCompletedEventHandlerMethod);
        }

        /// <summary>
        /// Fires the event taking "EventsArgs" instance as parameter
        /// Just call this method where you want to fire the event
        /// </summary>
        /// <param name="e"></param>
        public void RaiseProcessCompletedEvent(EventsArgs e)
        {
            //lock (syncLock)
            {
                if (processCompletedEvent != null)
                {
                    processCompletedEvent(this, e); //Fires the event
                }
            }
        }

    }

    /// <summary>
    /// Contains data to be passed through the event
    /// </summary>
    public class EventsArgs : EventArgs
    {

        public string[] paramsData { get; set; }
        public DataSet ds { get; set; }

        public EventsArgs(DataSet ds, params string[] paramsData)
        {
            this.paramsData = paramsData;
            this.ds = ds;
        }

        public EventsArgs()
        {
            //no data required to be passed
        }

        public EventsArgs(params string[] paramsData)
        {
            //no data required to be passed
            this.paramsData = paramsData;
        }

    }
}
