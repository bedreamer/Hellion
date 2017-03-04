# Hellion Packets

In this documentation, you will find every FlyFF packets structure, client and servers.

## Packets list

| Packet Name | Packet Value | Source | Destination | Description |
| ----------- | ------------ | ----------- | ----------- | ----------- |
| [CERTIFY](/docs/packets/Login.md#certify) | `0x000000FC` | Client | Login Server |The client sends to the server a login request. |
| [SRVR_LIST](/docs/packets/Login.md#srvr_list) | `x000000FD` | Login Server  | Client | Send the list of available servers to the client. |
| [ERROR](/docs/packets/Login.md#error) | `x000000FE` | Login Server  | Client | Send an error message to the client. |


## Documentation

[packetType]: /src/Hellion.Core/Data/Headers/PacketType.cs
[snapshotType]: /src/Hellion.Core/Data/Headers/SnapshotType.cs