﻿NDSummary.OnToolTipsLoaded("CSharpClass:SLMP.SlmpClient",{21:"<div class=\"NDToolTip TClass LCSharp\"><div class=\"NDClassPrototype\" id=\"NDClassPrototype21\"><div class=\"CPEntry TClass Current\"><div class=\"CPModifiers\"><span class=\"SHKeyword\">public</span></div><div class=\"CPName\"><span class=\"Qualifier\">SLMP.</span>&#8203;SlmpClient</div></div></div><div class=\"TTSummary\">This class exposes functionality to connect and manage SLMP-compatible devices.</div></div>",23:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype23\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private readonly byte</span>[] HEADER</div></div><div class=\"TTSummary\">This `HEADER` array contains the shared (header) data between commands that are supported in this library.</div></div>",24:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype24\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> SlmpConfig config</div></div></div>",25:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype25\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> TcpClient client</div></div></div>",26:"<div class=\"NDToolTip TVariable LCSharp\"><div id=\"NDPrototype26\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> NetworkStream? stream</div></div></div>",28:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype28\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> SlmpClient(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">SlmpConfig&nbsp;</td><td class=\"PName last\">cfg</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Initializes a new instance of the SlmpClient class.</div></div>",29:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype29\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public void</span> Connect()</div></div><div class=\"TTSummary\">Connects to the address specified in the config.</div></div>",30:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype30\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public void</span> Disconnect()</div></div></div>",31:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype31\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public bool</span> ReadDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">BitDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Reads a single Bit from a given `BitDevice` and returns a `bool`.</div></div>",32:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype32\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public bool</span>[] ReadDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">BitDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">count</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Reads from a given `BitDevice` and returns an array of `bool`s.&nbsp; Note that there\'s a limit on how many registers can be read at a time.</div></div>",33:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype33\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public ushort</span> ReadDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Reads a single Word from a the given `WordDevice` and returns an `ushort`.</div></div>",34:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype34\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public ushort</span>[] ReadDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">count</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Reads from a given `WordDevice` and returns an array of `ushort`s.&nbsp; Note that there\'s a limit on how many registers can be read at a time.</div></div>",35:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype35\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> WriteDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">BitDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">bool</span>&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Writes a single `Bit` to a given `BitDevice`.</div></div>",36:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype36\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> WriteDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">BitDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">bool</span>[]&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Writes an array of `bool`s to a given `BitDevice`.&nbsp; Note that there\'s a limit on how many registers can be written at a time.</div></div>",37:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype37\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> WriteDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Writes a single `ushort` to a given `WordDevice`.</div></div>",38:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype38\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> WriteDevice(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>[]&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Writes an array of `ushort`s to a given `WordDevice`.&nbsp; Note that there\'s a limit on how many registers can be written at a time.</div></div>",39:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype39\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public void</span> WriteString(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">string</span>&nbsp;</td><td class=\"PName last\">text</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Writes the given string to the specified device as a null terminated string.&nbsp; Note that there\'s a limit on how many registers can be written at a time.</div></div>",40:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype40\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public string</span> ReadString(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">len</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Reads a string with the length `len` from the specified `WordDevice`. Note that this function reads the string at best two chars, ~500 times in a second.&nbsp; Meaning it can only read ~1000 chars per second.&nbsp; Note that there\'s a limit on how many registers can be read at a time.</div></div>",41:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype41\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">public</span> T? ReadStruct&lt;T&gt;(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\">WordDevice&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">addr</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">where</span> T : <span class=\"SHKeyword\">struct</span></div></div><div class=\"TTSummary\">Read from a `WordDevice` to create a C# structure.&nbsp; The target structure can only contain very primitive data types.&nbsp; Supported data types: bool: 2 bytes, 0 for `False` anything else for `True` ushort: 2 bytes (UInt16) short: 2 bytes (Int16) uint: 4 bytes (UInt32) int: 4 bytes (Int32) string: arbitrary long, must have an `SLMPStringAttribute`</div></div>",42:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype42\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public bool</span> SelfTest()</div></div></div>",43:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype43\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">public bool</span> Connected()</div></div><div class=\"TTSummary\">Query the connection status.</div></div>",44:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype44\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private void</span> CheckConnection()</div></div></div>",45:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype45\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private static ushort</span> GetSubcommand(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">dynamic</span>&nbsp;</td><td class=\"PName last\">type</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Gets the subcommand for a given `(Bit/Word)Device`.</div></div>",46:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype46\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private byte</span>[] ReceiveBytes(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">int</span>&nbsp;</td><td class=\"PName last\">count</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">This function exists because `NetworkStream` doesn\'t have a `recv_exact` method.</div></div>",47:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype47\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private</span> List&lt;<span class=\"SHKeyword\">byte</span>&gt; ReceiveResponse()</div></div><div class=\"TTSummary\">Receives the response and returns the raw response data.</div></div>",48:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype48\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SendReadDeviceCommand(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">dynamic</span>&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">adr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">cnt</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Sends the read device command.</div></div>",49:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype49\" class=\"NDPrototype WideForm\"><div class=\"PSection PParameterSection CStyle\"><table><tr><td class=\"PBeforeParameters\"><span class=\"SHKeyword\">private void</span> SendWriteDeviceCommand(</td><td class=\"PParametersParentCell\"><table class=\"PParameters\"><tr><td class=\"PType first\"><span class=\"SHKeyword\">dynamic</span>&nbsp;</td><td class=\"PName last\">device,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">adr,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">ushort</span>&nbsp;</td><td class=\"PName last\">cnt,</td></tr><tr><td class=\"PType first\"><span class=\"SHKeyword\">byte</span>[]&nbsp;</td><td class=\"PName last\">data</td></tr></table></td><td class=\"PAfterParameters\">)</td></tr></table></div></div><div class=\"TTSummary\">Sends the write device command.</div></div>",50:"<div class=\"NDToolTip TFunction LCSharp\"><div id=\"NDPrototype50\" class=\"NDPrototype\"><div class=\"PSection PPlainSection\"><span class=\"SHKeyword\">private void</span> SendSelfTestCommand()</div></div></div>"});