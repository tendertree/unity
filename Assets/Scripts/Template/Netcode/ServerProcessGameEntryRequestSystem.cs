using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
public partial struct ServerProcessGameEntryRequestSystem : ISystem
{
    // Start is called before the first frame update
    void OnCreate(ref SystemState state)
    {
        var build = new EntityQueryBuilder(Allocator.Temp).WithAll<SimpleFoodRequest, ReceiveRpcCommandRequest>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
