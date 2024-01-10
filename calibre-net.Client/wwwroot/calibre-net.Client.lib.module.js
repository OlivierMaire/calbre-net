
export function afterWebStarted(blazor) {

  // console.log("lock and load");

  // blazor.registerCustomEventType('customevent', {
  //   browserEventName: 'click',
  // });

  // console.log("lock and load again");

  blazor.registerCustomEventType('themechangedevent', {
    browserEventName: 'themeChanged',
    createEventArgs: event => {
      // console.log("event here ");
      // console.log(event.detail);
      return {
        isDarkMode: event.detail.isDarkMode
      };
    }
  });
}