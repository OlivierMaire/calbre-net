export const calibreMessage = {
    send: (value) => {
        // console.log("sending Calibre Message");
        // console.log(value);

      const event = new CustomEvent('calibreMessage', { bubbles: true, 
        detail: value });
      document.querySelector(".calibreMessageHandler").dispatchEvent(event);
    }
  }
    ;