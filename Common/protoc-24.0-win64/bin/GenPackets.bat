protoc -I=./ --csharp_out=./ ./Protocol.proto


@REM START ../../PacketGenerator/bin/Debug/PacketGenerator.exe ../../PacketGenerator/PDL.xml
@REM XCOPY /Y GenPackets.cs "../../DummyClient/Packet"
@REM XCOPY /Y GenPackets.cs "../../../Client/Assets/Scripts/Packet"
@REM XCOPY /Y GenPackets.cs "../../Server/Packet"

@REM XCOPY /Y ClientPacketManager.cs "../../DummyClient/Packet"
@REM XCOPY /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
@REM XCOPY /Y ServerPacketManager.cs "../../Server/Packet"