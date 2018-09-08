package md5c90e4a1d4c1d83b48682a7e35718fecb;


public class MainActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_Button_Language_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Flags_Click:(Landroid/view/View;)V:__export__\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("ReLearn.MainActivity, ReLearn", MainActivity.class, __md_methods);
	}


	public MainActivity ()
	{
		super ();
		if (getClass () == MainActivity.class)
			mono.android.TypeManager.Activate ("ReLearn.MainActivity, ReLearn", "", this, new java.lang.Object[] {  });
	}


	public void Button_Language_Click (android.view.View p0)
	{
		n_Button_Language_Click (p0);
	}

	private native void n_Button_Language_Click (android.view.View p0);


	public void Button_Flags_Click (android.view.View p0)
	{
		n_Button_Flags_Click (p0);
	}

	private native void n_Button_Flags_Click (android.view.View p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
