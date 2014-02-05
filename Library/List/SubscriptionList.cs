﻿using System.Xml;

namespace Recurly
{
    public class SubscriptionList : RecurlyList<Subscription>
    {
        internal SubscriptionList(string baseUrl) : base(Client.HttpRequestMethod.Get, baseUrl)
        {
        }

        public override RecurlyList<Subscription> Start
        {
            get { return new SubscriptionList(StartUrl); }
        }

        public override RecurlyList<Subscription> Next
        {
            get { return new SubscriptionList(NextUrl); }
        }

        public override RecurlyList<Subscription> Prev
        {
            get { return new SubscriptionList(PrevUrl); }
        }

        internal override void ReadXml(XmlTextReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name == "subscriptions" && reader.NodeType == XmlNodeType.EndElement)
                    break;

                if (reader.NodeType == XmlNodeType.Element && reader.Name == "subscription")
                {
                    Add(new Subscription(reader));
                }
            }
        }

        /// <summary>
        /// Returns a list of recurly subscriptions
        /// 
        /// A subscription will belong to more than one state.
        /// </summary>
        /// <param name="state">State of subscriptions to return, defaults to "live"</param>
        /// <returns></returns>
        public static SubscriptionList GetSubscriptions(Subscription.SubscriptionState state = Subscription.SubscriptionState.Live)
        {
            return new SubscriptionList(Subscription.UrlPrefix + "?state=" + state.ToString().EnumNameToTransportCase());
        }
    }
}