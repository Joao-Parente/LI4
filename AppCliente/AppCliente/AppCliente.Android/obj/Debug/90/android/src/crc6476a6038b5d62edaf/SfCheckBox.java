package crc6476a6038b5d62edaf;


public class SfCheckBox
	extends android.widget.CompoundButton
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_getPaddingLeft:()I:GetGetPaddingLeftHandler\n" +
			"n_getPaddingRight:()I:GetGetPaddingRightHandler\n" +
			"n_isEnabled:()Z:GetIsEnabledHandler\n" +
			"n_setEnabled:(Z)V:GetSetEnabled_ZHandler\n" +
			"n_onSaveInstanceState:()Landroid/os/Parcelable;:GetOnSaveInstanceStateHandler\n" +
			"n_setPadding:(IIII)V:GetSetPadding_IIIIHandler\n" +
			"n_onRestoreInstanceState:(Landroid/os/Parcelable;)V:GetOnRestoreInstanceState_Landroid_os_Parcelable_Handler\n" +
			"n_setBackgroundColor:(I)V:GetSetBackgroundColor_IHandler\n" +
			"n_onCreateDrawableState:(I)[I:GetOnCreateDrawableState_IHandler\n" +
			"n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\n" +
			"n_onDraw:(Landroid/graphics/Canvas;)V:GetOnDraw_Landroid_graphics_Canvas_Handler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android", SfCheckBox.class, __md_methods);
	}


	public SfCheckBox (android.content.Context p0)
	{
		super (p0);
		if (getClass () == SfCheckBox.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SfCheckBox (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == SfCheckBox.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfCheckBox (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == SfCheckBox.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public SfCheckBox (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == SfCheckBox.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox, Syncfusion.Buttons.XForms.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public int getPaddingLeft ()
	{
		return n_getPaddingLeft ();
	}

	private native int n_getPaddingLeft ();


	public int getPaddingRight ()
	{
		return n_getPaddingRight ();
	}

	private native int n_getPaddingRight ();


	public boolean isEnabled ()
	{
		return n_isEnabled ();
	}

	private native boolean n_isEnabled ();


	public void setEnabled (boolean p0)
	{
		n_setEnabled (p0);
	}

	private native void n_setEnabled (boolean p0);


	public android.os.Parcelable onSaveInstanceState ()
	{
		return n_onSaveInstanceState ();
	}

	private native android.os.Parcelable n_onSaveInstanceState ();


	public void setPadding (int p0, int p1, int p2, int p3)
	{
		n_setPadding (p0, p1, p2, p3);
	}

	private native void n_setPadding (int p0, int p1, int p2, int p3);


	public void onRestoreInstanceState (android.os.Parcelable p0)
	{
		n_onRestoreInstanceState (p0);
	}

	private native void n_onRestoreInstanceState (android.os.Parcelable p0);


	public void setBackgroundColor (int p0)
	{
		n_setBackgroundColor (p0);
	}

	private native void n_setBackgroundColor (int p0);


	public int[] onCreateDrawableState (int p0)
	{
		return n_onCreateDrawableState (p0);
	}

	private native int[] n_onCreateDrawableState (int p0);


	public void onLayout (boolean p0, int p1, int p2, int p3, int p4)
	{
		n_onLayout (p0, p1, p2, p3, p4);
	}

	private native void n_onLayout (boolean p0, int p1, int p2, int p3, int p4);


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
