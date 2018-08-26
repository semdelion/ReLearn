package md5c90e4a1d4c1d83b48682a7e35718fecb;


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
		mono.android.Runtime.register ("ReLearn.GUI, ReLearn", GUI.class, __md_methods);
	}


	public GUI ()
	{
		super ();
		if (getClass () == GUI.class)
			mono.android.TypeManager.Activate ("ReLearn.GUI, ReLearn", "", this, new java.lang.Object[] {  });
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
