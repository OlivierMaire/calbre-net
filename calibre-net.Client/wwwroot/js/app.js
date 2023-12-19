window.blazorCulture = {
  get: () => window.localStorage['BlazorCulture'],
  set: (value) => window.localStorage['BlazorCulture'] = value
};

window.blazorDarkTheme = {
  get: () => {
    let result = window.localStorage['BlazorDarkTheme'];
    if (!result) {
      // get system pref.
      if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
        // dark mode
        result = true;
      }
      else {
        result = false;
      }
    }
    return result.toString();

  },
  set: (value) => window.localStorage['BlazorDarkTheme'] = value
};