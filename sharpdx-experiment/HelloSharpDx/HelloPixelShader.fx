float4 GreenPS() : COLOR
{
	return float4(0.0f, 1.0f, 0.0f, 1.0f);
}

float4 RedPS() : COLOR
{
	return float4(1.0f, 0.0f, 0.0f, 1.0f);
}

technique FillWithGreenAndStrokeWithRedT 
{
	pass FillWithGreenP
	{
		pixelShader = compile ps_2_0 GreenPS();
		FillMode = Solid;
	}

	pass StrokeWithRedP
	{
		pixelShader = compile ps_2_0 RedPS();
		FillMode = Wireframe;
	}
}
