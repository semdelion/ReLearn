package md5bb1bd5097c2fcdc9a5f20c0b9b37d5ac;


public class Languages
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_Button_Languages_Add_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_Learn_Click:(Landroid/view/View;)V:__export__\n" +
			"n_Button_Languages_Repeat_Click:(Landroid/view/View;)V:__export__\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onPrepareOptionsMenu:(Landroid/view/Menu;)Z:GetOnPrepareOptionsMenu_Landroid_view_Menu_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_attachBaseContext:(Landroid/content/Context;)V:GetAttachBaseContext_Landroid_content_Context_Handler\n" +
			"";
		mono.android.Runtime.register ("ReLearn.Languages.Languages, ReLearn", Languages.class, __md_methods);
	}


	public Languages ()
	{
		super ();
		if (getClass () == Languages.class)
			mono.android.TypeManager.Activate ("ReLearn.Languages.Languages, ReLearn", "", this, new java.lang.Object[] {  });
	}


	public void Button_Languages_Add_Click (android.view.View p0)
	{
		n_Button_Languages_Add_Click (p0);
	}

	private native void n_Button_Languages_Add_Click (android.view.View p0);


	public void Button_Languages_Learn_Click (android.view.View p0)
	{
		n_Button_Languages_Learn_Click (p0);
	}

	private native void n_Button_Languages_Learn_Click (android.view.View p0);


	public void Button_Languages_Repeat_Click (android.view.View p0)
	{
		n_Button_Languages_Repeat_Click (p0);
	}

	private native void n_Button_Languages_Repeat_Click (android.view.View p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public boolean onPrepareOptionsMenu (android.view.Menu p0)
	{
		return n_onPrepareOptionsMenu (p0);
	}

	private native boolean n_onPrepareOptionsMenu (android.view.Menu p0);


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
