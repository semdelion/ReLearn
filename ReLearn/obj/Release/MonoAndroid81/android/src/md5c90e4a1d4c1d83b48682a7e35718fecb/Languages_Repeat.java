package md5c90e4a1d4c1d83b48682a7e35718fecb;


public class Languages_Repeat
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_Button_Speak_Languages_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_1_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_2_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_3_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_4_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_Next_Click:(Landroid/view/View;)V:__export__\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_attachBaseContext:(Landroid/content/Context;)V:GetAttachBaseContext_Landroid_content_Context_Handler\n" +
			"";
		mono.android.Runtime.register ("ReLearn.Languages_Repeat, ReLearn", Languages_Repeat.class, __md_methods);
	}


	public Languages_Repeat ()
	{
		super ();
		if (getClass () == Languages_Repeat.class)
			mono.android.TypeManager.Activate ("ReLearn.Languages_Repeat, ReLearn", "", this, new java.lang.Object[] {  });
	}


	public void Button_Speak_Languages_Click (android.view.View p0)
	{
		n_Button_Speak_Languages_Click (p0);
	}

	private native void n_Button_Speak_Languages_Click (android.view.View p0);


	public void Button_Languages_1_Click (android.view.View p0)
	{
		n_Button_Languages_1_Click (p0);
	}

	private native void n_Button_Languages_1_Click (android.view.View p0);


	public void Button_Languages_2_Click (android.view.View p0)
	{
		n_Button_Languages_2_Click (p0);
	}

	private native void n_Button_Languages_2_Click (android.view.View p0);


	public void Button_Languages_3_Click (android.view.View p0)
	{
		n_Button_Languages_3_Click (p0);
	}

	private native void n_Button_Languages_3_Click (android.view.View p0);


	public void Button_Languages_4_Click (android.view.View p0)
	{
		n_Button_Languages_4_Click (p0);
	}

	private native void n_Button_Languages_4_Click (android.view.View p0);


	public void Button_Languages_Next_Click (android.view.View p0)
	{
		n_Button_Languages_Next_Click (p0);
	}

	private native void n_Button_Languages_Next_Click (android.view.View p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);


	public void attachBaseContext (android.content.Context p0)
	{
		n_attachBaseContext (p0);
	}

	private native void n_attachBaseContext (android.content.Context p0);

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
