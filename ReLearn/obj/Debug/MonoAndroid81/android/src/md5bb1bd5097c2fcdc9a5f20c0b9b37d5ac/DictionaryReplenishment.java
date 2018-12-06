package md5bb1bd5097c2fcdc9a5f20c0b9b37d5ac;


public class DictionaryReplenishment
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_Button_Languages_Add_DictionaryReplenishment:(Landroid/view/View;)V:__export__\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onPrepareOptionsMenu:(Landroid/view/Menu;)Z:GetOnPrepareOptionsMenu_Landroid_view_Menu_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"";
		mono.android.Runtime.register ("ReLearn.Languages.DictionaryReplenishment, ReLearn", DictionaryReplenishment.class, __md_methods);
	}


	public DictionaryReplenishment ()
	{
		super ();
		if (getClass () == DictionaryReplenishment.class)
			mono.android.TypeManager.Activate ("ReLearn.Languages.DictionaryReplenishment, ReLearn", "", this, new java.lang.Object[] {  });
	}


	public void Button_Languages_Add_DictionaryReplenishment (android.view.View p0)
	{
		n_Button_Languages_Add_DictionaryReplenishment (p0);
	}

	private native void n_Button_Languages_Add_DictionaryReplenishment (android.view.View p0);


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
