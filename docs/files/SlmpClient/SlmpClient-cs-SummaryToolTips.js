﻿NDSummary.OnToolTipsLoaded("File:SlmpClient/SlmpClient.cs",{32:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype32\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\"><span class=\"Qualifier\">SLMP.</span>&#8203;SlmpClient</div></div></div><div class=\"TTSummary\">This class exposes functionality to connect and manage SLMP-compatible devices.</div></div>",35:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype35\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private readonly byte</span>[] HEADER</div></div><div class=\"TTSummary\">This `HEADER` array contains the shared (header) data between commands that are supported in this library.</div></div>",36:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype36\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> SlmpConfig _config</div></div></div>",37:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype37\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> TcpClient _client</div></div></div>",38:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype38\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> NetworkStream? _stream</div></div></div>",40:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype40\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> SlmpClient(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">SlmpConfig&nbsp;</td><td class=\"PName last\">cfg</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Initializes a new instance of the SlmpClient class.</div></div>",41:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype41\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public void</span> Connect()</div></div></div>",42:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype42\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public void</span> Disconnect()</div></div><div class=\"TTSummary\">Attempt to close the socket connection.</div></div>",43:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype43\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public bool</span> SelfTest()</div></div></div>",52:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype52\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public bool</span> Connected()</div></div><div class=\"TTSummary\">Query the connection status.</div></div>",53:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype53\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private void</span> CheckConnection()</div></div></div>",56:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype56\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private byte</span>[] ReceiveBytes(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">count</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">This function exists because `NetworkStream` doesn\'t have a `recv_exact` method.</div></div>",57:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype57\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> List&lt;<span class=\"SHKeyword\">byte</span>&gt; ReceiveResponse()</div></div><div class=\"TTSummary\">Receives the response and returns the raw response data.</div></div>",58:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype58\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SendReadDeviceCommand(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">dynamic</span>&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">adr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">cnt</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Sends the read device command.</div></div>",59:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype59\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SendWriteDeviceCommand(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">dynamic</span>&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">adr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">cnt,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">byte</span>[]&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Sends the write device command.</div></div>",60:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype60\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private void</span> SendSelfTestCommand()</div></div><div class=\"TTSummary\">Sends the `SelfTest` command.</div></div>"});