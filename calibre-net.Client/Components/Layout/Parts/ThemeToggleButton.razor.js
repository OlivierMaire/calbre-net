
export const blazorDarkTheme = {
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
    set: (value) => {
    //   console.log("setting theme to " + value);
      window.localStorage['BlazorDarkTheme'] = value;
      const event = new CustomEvent('themeChanged', { bubbles: true, detail: { isDarkMode : value } });
      document.querySelector(".themeChangedEventHandler").dispatchEvent(event);
    }
  }
    ;