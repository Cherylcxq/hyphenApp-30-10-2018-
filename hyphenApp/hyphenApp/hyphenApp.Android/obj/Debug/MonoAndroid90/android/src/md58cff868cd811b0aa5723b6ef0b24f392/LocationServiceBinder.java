package md58cff868cd811b0aa5723b6ef0b24f392;


public class LocationServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("hyphenApp.Droid.Services.LocationServiceBinder, hyphenApp.Android", LocationServiceBinder.class, __md_methods);
	}


	public LocationServiceBinder ()
	{
		super ();
		if (getClass () == LocationServiceBinder.class)
			mono.android.TypeManager.Activate ("hyphenApp.Droid.Services.LocationServiceBinder, hyphenApp.Android", "", this, new java.lang.Object[] {  });
	}

	public LocationServiceBinder (md58cff868cd811b0aa5723b6ef0b24f392.LocationService p0)
	{
		super ();
		if (getClass () == LocationServiceBinder.class)
			mono.android.TypeManager.Activate ("hyphenApp.Droid.Services.LocationServiceBinder, hyphenApp.Android", "hyphenApp.Droid.Services.LocationService, hyphenApp.Android", this, new java.lang.Object[] { p0 });
	}

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
