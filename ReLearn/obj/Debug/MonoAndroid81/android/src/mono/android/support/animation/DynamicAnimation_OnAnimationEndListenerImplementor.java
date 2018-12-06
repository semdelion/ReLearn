package mono.android.support.animation;


public class DynamicAnimation_OnAnimationEndListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.animation.DynamicAnimation.OnAnimationEndListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationEnd:(Landroid/support/animation/DynamicAnimation;ZFF)V:GetOnAnimationEnd_Landroid_support_animation_DynamicAnimation_ZFFHandler:Android.Support.Animation.DynamicAnimation/IOnAnimationEndListenerInvoker, Xamarin.Android.Support.Dynamic.Animation\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Animation.DynamicAnimation+IOnAnimationEndListenerImplementor, Xamarin.Android.Support.Dynamic.Animation", DynamicAnimation_OnAnimationEndListenerImplementor.class, __md_methods);
	}


	public DynamicAnimation_OnAnimationEndListenerImplementor ()
	{
		super ();
		if (getClass () == DynamicAnimation_OnAnimationEndListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Animation.DynamicAnimation+IOnAnimationEndListenerImplementor, Xamarin.Android.Support.Dynamic.Animation", "", this, new java.lang.Object[] {  });
	}


	public void onAnimationEnd (android.support.animation.DynamicAnimation p0, boolean p1, float p2, float p3)
	{
		n_onAnimationEnd (p0, p1, p2, p3);
	}

	private native void n_onAnimationEnd (android.support.animation.DynamicAnimation p0, boolean p1, float p2, float p3);

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
