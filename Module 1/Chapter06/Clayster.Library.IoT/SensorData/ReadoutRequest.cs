﻿using System;
using System.Xml;
using System.Collections.Generic;
using Clayster.Library.Internet;
using Clayster.Library.Internet.HTTP;

namespace Clayster.Library.IoT.SensorData
{
	/// <summary>
	/// Represents a request for sensor data.
	/// </summary>
	/// <remarks>
	/// © Clayster, 2014
	/// 
	/// Author: Peter Waher
	/// </remarks>
	[CLSCompliant (true)]
	[Serializable]
	public class ReadoutRequest
	{
		private SortedDictionary<string,bool> fields = null;
		private NodeReference[] nodes = null;
		private ReadoutType types = (ReadoutType)0;
		private DateTime from = DateTime.MinValue;
		private DateTime to = DateTime.MaxValue;
		private string serviceToken = string.Empty;
		private string deviceToken = string.Empty;
		private string userToken = string.Empty;

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Types">Readout types to read.</param>
		public ReadoutRequest (ReadoutType Types)
		{
			this.types = Types;
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Types">Readout types to read.</param>
		/// <param name="From">From what timestamp readout is desired.</param>
		/// <param name="To">To what timestamp readout is desired.</param>
		public ReadoutRequest (ReadoutType Types, DateTime From, DateTime To)
		{
			this.types = Types;
			this.from = From;
			this.to = To;
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Types">Readout types to read.</param>
		/// <param name="From">From what timestamp readout is desired.</param>
		/// <param name="To">To what timestamp readout is desired.</param>
		/// <param name="Nodes">Nodes to read.</param>
		public ReadoutRequest (ReadoutType Types, DateTime From, DateTime To, NodeReference[] Nodes)
		{
			this.types = Types;
			this.from = From;
			this.to = To;
			this.nodes = Nodes;
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Types">Readout types to read.</param>
		/// <param name="From">From what timestamp readout is desired.</param>
		/// <param name="To">To what timestamp readout is desired.</param>
		/// <param name="Nodes">Nodes to read.</param>
		/// <param name="Fields">Fields</param>
		public ReadoutRequest (ReadoutType Types, DateTime From, DateTime To, NodeReference[] Nodes, IEnumerable<string> Fields)
		{
			this.types = Types;
			this.from = From;
			this.to = To;
			this.nodes = Nodes;

			this.fields = new SortedDictionary<string, bool> ();

			foreach (string Field in Fields)
				this.fields [Field] = true;
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Types">Readout types to read.</param>
		/// <param name="From">From what timestamp readout is desired.</param>
		/// <param name="To">To what timestamp readout is desired.</param>
		/// <param name="Nodes">Nodes to read.</param>
		/// <param name="Fields">Fields</param>
		/// <param name="ServiceToken">Service Token</param>
		/// <param name="DeviceToken">Device Token</param>
		/// <param name="UserToken">User Token</param>
		public ReadoutRequest (ReadoutType Types, DateTime From, DateTime To, NodeReference[] Nodes, IEnumerable<string> Fields,
		                       string ServiceToken, string DeviceToken, string UserToken)
		{
			this.types = Types;
			this.from = From;
			this.to = To;
			this.nodes = Nodes;
			this.serviceToken = ServiceToken;
			this.deviceToken = DeviceToken;
			this.userToken = UserToken;

			this.fields = new SortedDictionary<string, bool> ();

			foreach (string Field in Fields)
				this.fields [Field] = true;
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Request">HTTP Request</param>
		public ReadoutRequest (HttpServerRequest Request)
		{
			string NodeId = string.Empty;
			string CacheType = string.Empty;
			string SourceId = string.Empty;
			bool b;

			foreach (KeyValuePair<string,string> Parameter in Request.Query)
			{
				switch (Parameter.Key.ToLower ())
				{
					case "nodeid":
						NodeId = Parameter.Value;
						break;

					case "cachetype":
						CacheType = Parameter.Value;
						break;

					case "sourceid":
						SourceId = Parameter.Value;
						break;

					case "all":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types = ReadoutType.All;
						else
							this.types = (ReadoutType)0;
						break;

					case "historical":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValues;
						else
							this.types &= ~ReadoutType.HistoricalValues;

						break;

					case "from":
						if (!XmlUtilities.TryParseDateTimeXml (Parameter.Value, out this.from))
							this.from = DateTime.MinValue;
						break;

					case "to":
						if (!XmlUtilities.TryParseDateTimeXml (Parameter.Value, out this.to))
							this.from = DateTime.MaxValue;
						break;

					case "when":
						throw new HttpException (HttpStatusCode.ClientError_BadRequest);	// Not supported through HTTP interface.
						
					case "servicetoken":
						this.serviceToken = Parameter.Value;
						break;

					case "devicetoken":
						this.deviceToken = Parameter.Value;
						break;

					case "usertoken":
						this.userToken = Parameter.Value;
						break;

					case "momentary":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.MomentaryValues;
						else
							this.types &= ~ReadoutType.MomentaryValues;
						break;

					case "peak":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.PeakValues;
						else
							this.types &= ~ReadoutType.PeakValues;
						break;

					case "status":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.StatusValues;
						else
							this.types &= ~ReadoutType.StatusValues;
						break;

					case "computed":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.Computed;
						else
							this.types &= ~ReadoutType.Computed;
						break;

					case "identity":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.Identity;
						else
							this.types &= ~ReadoutType.Identity;
						break;

					case "historicalsecond":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesSecond;
						else
							this.types &= ~ReadoutType.HistoricalValuesSecond;
						break;

					case "historicalminute":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesMinute;
						else
							this.types &= ~ReadoutType.HistoricalValuesMinute;
						break;

					case "historicalhour":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesHour;
						else
							this.types &= ~ReadoutType.HistoricalValuesHour;
						break;

					case "historicalday":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesDay;
						else
							this.types &= ~ReadoutType.HistoricalValuesDay;
						break;

					case "historicalweek":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesWeek;
						else
							this.types &= ~ReadoutType.HistoricalValuesWeek;
						break;

					case "historicalmonth":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesMonth;
						else
							this.types &= ~ReadoutType.HistoricalValuesMonth;
						break;

					case "historicalquarter":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesQuarter;
						else
							this.types &= ~ReadoutType.HistoricalValuesQuarter;
						break;

					case "historicalyear":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesYear;
						else
							this.types &= ~ReadoutType.HistoricalValuesYear;
						break;

					case "historicalother":
						if (XmlUtilities.TryParseBoolean (Parameter.Value, out b) && b)
							this.types |= ReadoutType.HistoricalValuesOther;
						else
							this.types &= ~ReadoutType.HistoricalValuesOther;
						break;

					default:
						if (this.fields == null)
							this.fields = new SortedDictionary<string, bool> ();

						this.fields [Parameter.Key] = true;
						break;
				}
			}

			if ((int)this.types == 0)
				this.types = ReadoutType.All;	// If no types specified, all types are implicitly implied.

			if (!string.IsNullOrEmpty (NodeId))
				this.nodes = new NodeReference[]{ new NodeReference (NodeId, CacheType, SourceId) };
		}

		/// <summary>
		/// Represents a request for sensor data.
		/// </summary>
		/// <param name="Req">Readout request element.</param>
		public ReadoutRequest (XmlElement Req)
		{
			this.from = XmlUtilities.GetAttribute (Req, "from", DateTime.MinValue);
			this.to = XmlUtilities.GetAttribute (Req, "to", DateTime.MaxValue);
			this.serviceToken = XmlUtilities.GetAttribute (Req, "serviceToken", string.Empty);
			this.deviceToken = XmlUtilities.GetAttribute (Req, "deviceToken", string.Empty);
			this.userToken = XmlUtilities.GetAttribute (Req, "userToken", string.Empty);
			this.types = ParseReadoutType (Req);

			List<NodeReference> Nodes;
			List<string> FieldNames;

			ParseNodesAndFieldNames (Req, out Nodes, out FieldNames);

			if (Nodes != null)
				this.nodes = Nodes.ToArray ();

			if (FieldNames != null)
				this.SetFields (FieldNames.ToArray ());
		}

		/// <summary>
		/// Parses readout types from an XML element.
		/// </summary>
		/// <returns>The readout types found.</returns>
		/// <param name="Element">XML request element.</param>
		public static ReadoutType ParseReadoutType (XmlElement Element)
		{
			ReadoutType Types = (ReadoutType)0;

			if (XmlUtilities.GetAttribute (Element, "all", false))
				Types = ReadoutType.All;
			else
			{
				if (XmlUtilities.GetAttribute (Element, "historical", false))
					Types |= ReadoutType.HistoricalValues;

				if (XmlUtilities.GetAttribute (Element, "momentary", false))
					Types |= ReadoutType.MomentaryValues;

				if (XmlUtilities.GetAttribute (Element, "peak", false))
					Types |= ReadoutType.PeakValues;

				if (XmlUtilities.GetAttribute (Element, "status", false))
					Types |= ReadoutType.StatusValues;

				if (XmlUtilities.GetAttribute (Element, "computed", false))
					Types |= ReadoutType.Computed;

				if (XmlUtilities.GetAttribute (Element, "identity", false))
					Types |= ReadoutType.Identity;

				if (XmlUtilities.GetAttribute (Element, "historicalSecond", false))
					Types |= ReadoutType.HistoricalValuesSecond;

				if (XmlUtilities.GetAttribute (Element, "historicalMinute", false))
					Types |= ReadoutType.HistoricalValuesMinute;

				if (XmlUtilities.GetAttribute (Element, "historicalHour", false))
					Types |= ReadoutType.HistoricalValuesHour;

				if (XmlUtilities.GetAttribute (Element, "historicalDay", false))
					Types |= ReadoutType.HistoricalValuesDay;

				if (XmlUtilities.GetAttribute (Element, "historicalWeek", false))
					Types |= ReadoutType.HistoricalValuesWeek;

				if (XmlUtilities.GetAttribute (Element, "historicalMonth", false))
					Types |= ReadoutType.HistoricalValuesMonth;

				if (XmlUtilities.GetAttribute (Element, "historicalQuarter", false))
					Types |= ReadoutType.HistoricalValuesQuarter;

				if (XmlUtilities.GetAttribute (Element, "historicalYear", false))
					Types |= ReadoutType.HistoricalValuesYear;

				if (XmlUtilities.GetAttribute (Element, "historicalOther", false))
					Types |= ReadoutType.HistoricalValuesOther;
			}

			return Types;
		}

		/// <summary>
		/// Extracts node and field (or parameter) references from an XML element.
		/// </summary>
		/// <param name="Element">Element.</param>
		/// <param name="Nodes">Nodes.</param>
		/// <param name="FieldNames">Field (or parameter) names.</param>
		public static void ParseNodesAndFieldNames (XmlElement Element, out List<NodeReference> Nodes, out List<string> FieldNames)
		{
			XmlElement E, E2;
			string NodeId;
			string SourceId;
			string CacheType;

			Nodes = null;
			FieldNames = null;

			foreach (XmlNode Node in Element.ChildNodes)
			{
				E = Node as XmlElement;
				if (E == null)
					continue;

				switch (E.LocalName)
				{
					case "node":
						NodeId = XmlUtilities.GetAttribute (E, "nodeId", string.Empty);
						SourceId = XmlUtilities.GetAttribute (E, "sourceId", string.Empty);
						CacheType = XmlUtilities.GetAttribute (E, "cacheType", string.Empty);

						if (Nodes == null)
							Nodes = new List<NodeReference> ();

						Nodes.Add (new NodeReference (NodeId, CacheType, SourceId));
						break;

					case "field":
					case "parameter":
					case "boolean":
					case "color":
					case "date":
					case "dateTime":
					case "double":
					case "duration":
					case "int":
					case "long":
					case "string":
					case "time":
						if (FieldNames == null)
							FieldNames = new List<string> ();

						FieldNames.Add (XmlUtilities.GetAttribute (E, "name", string.Empty));
						break;

					case "x":
						foreach (XmlNode Node2 in E.ChildNodes)
						{
							if (Node2.LocalName == "field" && (E2 = Node2 as XmlElement) != null)
							{
								if (FieldNames == null)
									FieldNames = new List<string> ();

								FieldNames.Add (XmlUtilities.GetAttribute (E2, "var", string.Empty));
							}
						}
						break;
				}
			}
		}

		/// <summary>
		/// Readout types requested
		/// </summary>
		/// <value>The types.</value>
		public ReadoutType Types
		{
			get { return this.types; } 
			internal set{ this.types = value; }
		}

		/// <summary>
		/// From what timepoint data is requested
		/// </summary>
		/// <value>From.</value>
		public DateTime From{ get { return this.from; } }

		/// <summary>
		/// To what timepoint data is requested
		/// </summary>
		/// <value>To.</value>
		public DateTime To{ get { return this.to; } }

		/// <summary>
		/// Service token, if any.
		/// </summary>
		public string ServiceToken{ get { return this.serviceToken; } }

		/// <summary>
		/// Device token, if any.
		/// </summary>
		public string DeviceToken{ get { return this.deviceToken; } }

		/// <summary>
		/// User token, if any.
		/// </summary>
		public string UserToken{ get { return this.userToken; } }

		/// <summary>
		/// Nodes requested to be read. If no nodes are explicitly requested, this array is null, and all nodes are implicitly requested.
		/// </summary>
		/// <value>Array of nodes explicitly requested.</value>
		public NodeReference[] Nodes
		{
			get { return this.nodes; } 
			internal set { this.nodes = value; }
		}

		/// <summary>
		/// Gets an array of fields to report. If null, it means all fields should be reported.
		/// </summary>
		/// <returns>Array of fields, or null if all fields are implied.</returns>
		public string[] GetFields ()
		{
			if (this.fields == null)
				return null;

			string[] Result = new string[this.fields.Count];
			this.fields.Keys.CopyTo (Result, 0);

			return Result;
		}

		/// <summary>
		/// Sets the fields to report in the readout. null means all fields should be reported.
		/// </summary>
		/// <param name="Fields">Fields.</param>
		internal void SetFields (string[] Fields)
		{
			if (Fields == null)
				this.fields = null;
			else
			{
				this.fields = new SortedDictionary<string, bool> ();

				foreach (string Field in Fields)
					this.fields [Field] = true;
			}
		}

		/// <summary>
		/// If a field is requested and should be reported.
		/// </summary>
		/// <returns>true, if the field is requested and should be reported, false otherwise.</returns>
		/// <param name="FieldName">Field name.</param>
		public bool ReportField (string FieldName)
		{
			if (this.fields == null)
				return true;
			else
				return this.fields.ContainsKey (FieldName);
		}

		/// <summary>
		/// If a timestamp is requested and should be reported.
		/// </summary>
		/// <returns>true, if the timestamp is requested and should be reported, false otherwise.</returns>
		/// <param name="Timestamp">Timestamp.</param>
		public bool ReportTimestamp (DateTime Timestamp)
		{
			return Timestamp >= this.from && Timestamp <= this.to;
		}

		/// <summary>
		/// If a node is requested and should be reported.
		/// </summary>
		/// <returns>true, if the node is requested and should be reported, false otherwise.</returns>
		/// <param name="NodeId">Node ID.</param>
		/// <param name="CacheType">Cache type.</param>
		/// <param name="SourceId">Source ID.</param>
		public bool ReportNode (string NodeId, string CacheType, string SourceId)
		{
			if (this.nodes == null)
				return true;

			foreach (NodeReference NodeRef in this.nodes)
			{
				if (NodeRef.NodeId != NodeId)
					continue;

				if (!string.IsNullOrEmpty (NodeRef.CacheType) && NodeRef.CacheType != CacheType)
					continue;

				if (!string.IsNullOrEmpty (NodeRef.SourceId) && NodeRef.SourceId != SourceId)
					continue;

				return true;
			}

			return false;
		}

		/// <summary>
		/// If a node is requested and should be reported.
		/// </summary>
		/// <returns>true, if the node is requested and should be reported, false otherwise.</returns>
		/// <param name="NodeId">Node ID.</param>
		/// <param name="SourceId">Source ID.</param>
		public bool ReportNode (string NodeId, string SourceId)
		{
			return ReportNode (NodeId, string.Empty, SourceId);
		}

		/// <summary>
		/// If a node is requested and should be reported.
		/// </summary>
		/// <returns>true, if the node is requested and should be reported, false otherwise.</returns>
		/// <param name="NodeId">Node ID.</param>
		public bool ReportNode (string NodeId)
		{
			return ReportNode (NodeId, string.Empty, string.Empty);
		}
	}
}

