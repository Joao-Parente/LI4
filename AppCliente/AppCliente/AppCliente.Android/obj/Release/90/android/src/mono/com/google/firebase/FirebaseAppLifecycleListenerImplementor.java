package mono.com.google.firebase;


public class FirebaseAppLifecycleListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.firebase.FirebaseAppLifecycleListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onDeleted:(Ljava/lang/String;Lcom/google/firebase/FirebaseOptions;)V:GetOnDeleted_Ljava_lang_String_Lcom_google_firebase_FirebaseOptions_Handler:Firebase.IFirebaseAppLifecycleListenerInvoker, Xamarin.Firebase.Common\n" +
			"";
		mono.android.Runtime.register ("Firebase.IFirebaseAppLifecycleListenerImplementor, Xamarin.Firebase.Common", FirebaseAppLifecycleListenerImplementor.class, __md_methods);
	}


	public FirebaseAppLifecycleListenerImplementor ()
	{
		super ();
		if (getClass () == FirebaseAppLifecycleListenerImplementor.class)
			mono.android.TypeManager.Activate ("Firebase.IFirebaseAppLifecycleListenerImplementor, Xamarin.Firebase.Common", "", this, new java.lang.Object[] {  });
	}


	public void onDeleted (java.lang.String p0, com.google.firebase.FirebaseOptions p1)
	{
		n_onDeleted (p0, p1);
	}

	private native void n_onDeleted (java.lang.String p0, com.google.firebase.FirebaseOptions p1);

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
