package crc64eeb36180fe6023e2;


public class GestureListener
	extends android.view.GestureDetector.SimpleOnGestureListener
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDown:(Landroid/view/MotionEvent;)Z:GetOnDown_Landroid_view_MotionEvent_Handler\n" +
			"n_onLongPress:(Landroid/view/MotionEvent;)V:GetOnLongPress_Landroid_view_MotionEvent_Handler\n" +
			"n_onDoubleTap:(Landroid/view/MotionEvent;)Z:GetOnDoubleTap_Landroid_view_MotionEvent_Handler\n" +
			"n_onSingleTapUp:(Landroid/view/MotionEvent;)Z:GetOnSingleTapUp_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android", GestureListener.class, __md_methods);
	}


	public GestureListener ()
	{
		super ();
		if (getClass () == GestureListener.class)
			mono.android.TypeManager.Activate ("Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android", "", this, new java.lang.Object[] {  });
	}

	public GestureListener (crc64eeb36180fe6023e2.SfEffectsViewRenderer p0)
	{
		super ();
		if (getClass () == GestureListener.class)
			mono.android.TypeManager.Activate ("Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android", "Syncfusion.XForms.Android.EffectsView.SfEffectsViewRenderer, Syncfusion.Core.XForms.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean onDown (android.view.MotionEvent p0)
	{
		return n_onDown (p0);
	}

	private native boolean n_onDown (android.view.MotionEvent p0);


	public void onLongPress (android.view.MotionEvent p0)
	{
		n_onLongPress (p0);
	}

	private native void n_onLongPress (android.view.MotionEvent p0);


	public boolean onDoubleTap (android.view.MotionEvent p0)
	{
		return n_onDoubleTap (p0);
	}

	private native boolean n_onDoubleTap (android.view.MotionEvent p0);


	public boolean onSingleTapUp (android.view.MotionEvent p0)
	{
		return n_onSingleTapUp (p0);
	}

	private native boolean n_onSingleTapUp (android.view.MotionEvent p0);

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
