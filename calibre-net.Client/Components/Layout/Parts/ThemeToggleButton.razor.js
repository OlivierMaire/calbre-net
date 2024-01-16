function setCookie(cname, cvalue, exdays) {
  const d = new Date();
  d.setTime(d.getTime() + (exdays*24*60*60*1000));
  let expires = "expires="+ d.toUTCString();
  document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}


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
      // add also a cookie
      setCookie("calibre-net_darkmode", value, 30);

      const event = new CustomEvent('themeChanged', { bubbles: true, detail: { isDarkMode : value } });
      document.querySelector(".themeChangedEventHandler").dispatchEvent(event);
    }
  }
    ;