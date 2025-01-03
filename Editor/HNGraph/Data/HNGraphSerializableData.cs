using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HN.Graph.Editor
{
    [Serializable]
    public class SerializableNodes : SerializedDictionary<string, HNGraphNode> {}
    
    [Serializable]
    public class SerializableConnections : SerializedDictionary<string, HNGraphConnection> {}

    [Serializable]
    public class SerializablePorts : SerializedDictionary<string, HNGraphPort> {}

    [Serializable]
    public class SerializableGroups : SerializedDictionary<string, HNGraphGroup> {}

    [Serializable]
    public class SerializableStickyNotes : SerializedDictionary<string, HNGraphStickyNote> {}

    [Serializable]
    public class SerializableRelayNodes : SerializedDictionary<string, HNGraphRelayNode> {}
}
