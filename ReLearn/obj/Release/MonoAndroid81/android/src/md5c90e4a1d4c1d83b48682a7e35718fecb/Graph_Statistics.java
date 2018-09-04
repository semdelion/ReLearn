package md5c90e4a1d4c1d83b48682a7e35718fecb;


public class Graph_Statistics
	extends android.view.View
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("ReLearn.Graph_Statistics, ReLearn", Graph_Statistics.class, __md_methods);
	}


	public Graph_Statistics (android.content.Context p0)
	{
		super (p0);
		if (getClass () == Graph_Statistics.class)
			mono.android.TypeManager.Activate ("ReLearn.Graph_Statistics, ReLearn", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public Graph_Statistics (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == Graph_Statistics.class)
			mono.android.TypeManager.Activate ("ReLearn.Graph_Statistics, ReLearn", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public Graph_Statistics (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == Graph_Statistics.class)
			mono.android.TypeManager.Activate ("ReLearn.Graph_Statistics, ReLearn", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public void onDraw (android.graphics.Canvas p0)
	{
		n_onDraw (p0);
	}

	private native void n_onDraw (android.graphics.Canvas p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
