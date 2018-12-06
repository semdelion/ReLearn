package mono.android.support.animation;


public class DynamicAnimation_OnAnimationUpdateListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.support.animation.DynamicAnimation.OnAnimationUpdateListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onAnimationUpdate:(Landroid/support/animation/DynamicAnimation;FF)V:GetOnAnimationUpdate_Landroid_support_animation_DynamicAnimation_FFHandler:Android.Support.Animation.DynamicAnimation/IOnAnimationUpdateListenerInvoker, Xamarin.Android.Support.Dynamic.Animation\n" +
			"";
		mono.android.Runtime.register ("Android.Support.Animation.DynamicAnimation+IOnAnimationUpdateListenerImplementor, Xamarin.Android.Support.Dynamic.Animation", DynamicAnimation_OnAnimationUpdateListenerImplementor.class, __md_methods);
	}


	public DynamicAnimation_OnAnimationUpdateListenerImplementor ()
	{
		super ();
		if (getClass () == DynamicAnimation_OnAnimationUpdateListenerImplementor.class)
			mono.android.TypeManager.Activate ("Android.Support.Animation.DynamicAnimation+IOnAnimationUpdateListenerImplementor, Xamarin.Android.Support.Dynamic.Animation", "", this, new java.lang.Object[] {  });
	}


	public void onAnimationUpdate (android.support.animation.DynamicAnimation p0, float p1, float p2)
	{
		n_onAnimationUpdate (p0, p1, p2);
	}

	private native void n_onAnimationUpdate (android.support.animation.DynamicAnimation p0, float p1, float p2);

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
