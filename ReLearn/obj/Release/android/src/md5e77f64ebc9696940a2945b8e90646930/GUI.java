package md5e77f64ebc9696940a2945b8e90646930;


public class GUI
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ReLearn.GUI, ReLearn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GUI.class, __md_methods);
	}


	public GUI ()
	{
		super ();
		if (getClass () == GUI.class)
			mono.android.TypeManager.Activate ("ReLearn.GUI, ReLearn, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

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
