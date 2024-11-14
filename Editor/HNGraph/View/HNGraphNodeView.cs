using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

namespace HN.Graph.Editor
{
    public class HNGraphNodeView : HNGraphBaseNodeView
    {
        public HNGraphNode NodeData => BaseNodeData as HNGraphNode;
        private Type passType;


        public HNGraphNodeView(HNGraphView graphView, HNGraphNode nodeData, HNGraphEdgeConnectorListener edgeConnectorListener) 
        : base(graphView, nodeData, edgeConnectorListener)
        {
            OnCreate();
        }

        public override void OnCreate()
        {
            passType = NodeData.GraphNodeClass.GetType();
            base.OnCreate();
        }

        protected override void DrawNode()
        {
            HNGraphNodeInfoAttribute info = passType.GetCustomAttribute<HNGraphNodeInfoAttribute>();
            title = info.NodeTitle;
            name = passType.Name;

            // string[] depths = info.MenuItem.Split('/');
            // foreach (string depth in depths)
            // {
            //     AddToClassList(depth.ToLower().Replace(' ', '-'));
            // }
        }

        protected override void DrawPorts()
        {
            PropertyInfo[] propertiesInfo = passType.GetProperties();
            foreach (var propertyInfo in propertiesInfo)
            {
                HNGraphPortInfoAttribute slotInfo = propertyInfo.GetCustomAttribute<HNGraphPortInfoAttribute>();
                if (slotInfo != null)
                {
                    HNGraphPort port = null;

                    foreach(var inputPort in BaseNodeData.InputPorts.Values)
                    {
                        if(inputPort.IsMatchWithAttribute(propertyInfo.PropertyType, slotInfo))
                        {
                            port = inputPort;
                        }
                    }

                    foreach(var outputPort in BaseNodeData.OutputPorts.Values)
                    {
                        if(outputPort.IsMatchWithAttribute(propertyInfo.PropertyType, slotInfo))
                        {
                            port = outputPort;
                        }
                    }

                    if(port == null)
                    {
                        port = new HNGraphPort(
                            BaseNodeData,
                            propertyInfo.PropertyType.FullName, 
                            slotInfo.PortName, 
                            slotInfo.PortDirection == HNGraphPortInfoAttribute.Direction.Input ? HNGraphPort.Direction.Input : HNGraphPort.Direction.Output, 
                            slotInfo.PortCapacity == HNGraphPortInfoAttribute.Capacity.Single ? HNGraphPort.Capacity.Single : HNGraphPort.Capacity.Multi
                            );
                        BaseNodeData.AddPort(port);
                    }

                    CreatePortView(port, slotInfo);
                }
            }
        }

        private void CreatePortView(HNGraphPort port, HNGraphPortInfoAttribute slotInfo)
        {
            HNGraphPortView portView = new HNGraphPortView(
                GraphView,
                port,
                this,
                slotInfo.PortName, 
                slotInfo.orientation == HNGraphPortInfoAttribute.Orientation.Horizontal ? Orientation.Horizontal : Orientation.Vertical,
                slotInfo.PortDirection == HNGraphPortInfoAttribute.Direction.Input ? Direction.Input : Direction.Output, 
                slotInfo.PortCapacity == HNGraphPortInfoAttribute.Capacity.Single ? Port.Capacity.Single : Port.Capacity.Multi,
                EdgeConnectorListener
                );
            if (slotInfo.PortDirection == HNGraphPortInfoAttribute.Direction.Input)
            {
                InputPortViews.Add(portView);
                if(slotInfo.orientation == HNGraphPortInfoAttribute.Orientation.Vertical)
                {
                    TopPortContainer.Add(portView);
                }
                else
                {
                    inputContainer.Add(portView);
                }
            }
            else
            {
                OutputPortViews.Add(portView);
                if (slotInfo.orientation == HNGraphPortInfoAttribute.Orientation.Vertical)
                {
                    BottomPortContainer.Add(portView);
                }
                else
                {
                    outputContainer.Add(portView);
                }
            }
        }

    }
}