package crc6476a6038b5d62edaf;


public class SfCheckBox_SfSavedState
	extends android.view.View.BaseSavedState
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_writeToParcel:(Landroid/os/Parcel;I)V:GetWriteToParcel_Landroid_os_Parcel_IHandler\n" +
			"";
		mono.android.Runtime.register ("Syncfusion.Android.Buttons.SfCheckBox+SfSavedState, Syncfusion.Buttons.XForms.Android", SfCheckBox_SfSavedState.class, __md_methods);
	}


	public SfCheckBox_SfSavedState (android.os.Parcel p0)
	{
		super (p0);
		if (getClass () == SfCheckBox_SfSavedState.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox+SfSavedState, Syncfusion.Buttons.XForms.Android", "Android.OS.Parcel, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public SfCheckBox_SfSavedState (android.os.Parcel p0, java.lang.ClassLoader p1)
	{
		super (p0, p1);
		if (getClass () == SfCheckBox_SfSavedState.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox+SfSavedState, Syncfusion.Buttons.XForms.Android", "Android.OS.Parcel, Mono.Android:Java.Lang.ClassLoader, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public SfCheckBox_SfSavedState (android.os.Parcelable p0)
	{
		super (p0);
		if (getClass () == SfCheckBox_SfSavedState.class)
			mono.android.TypeManager.Activate ("Syncfusion.Android.Buttons.SfCheckBox+SfSavedState, Syncfusion.Buttons.XForms.Android", "Android.OS.IParcelable, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public void writeToParcel (android.os.Parcel p0, int p1)
	{
		n_writeToParcel (p0, p1);
	}

	private native void n_writeToParcel (android.os.Parcel p0, int p1);

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
