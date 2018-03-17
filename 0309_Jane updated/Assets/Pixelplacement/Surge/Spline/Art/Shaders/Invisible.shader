/// <summary>
/// SURGE FRAMEWORK
/// Author: Bob Berkebile
/// Email: bobb@pixelplacement.com
/// </summary>

Shader "Invisible"
{
	SubShader
	{
		Tags
		{
			"RenderType"="Transparent" "Queue"="Transparent"
		}	

    	Pass
    	{
    		ZTest off
        	Blend SrcAlpha OneMinusSrcAlpha
        	Color (1,1,1,0)
    	}
	}
}