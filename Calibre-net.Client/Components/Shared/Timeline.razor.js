export function scrollToUnread()
{
   var unread =  document.querySelector(".mud-timeline-item:not(:has(div.mark-read))");
   var header = document.querySelector("header.mud-appbar");
//    unread.scrollIntoView();

    var headerOffset = header.offsetHeight;
    var elementPosition = unread.getBoundingClientRect().top;
    var offsetPosition = elementPosition - headerOffset;
  
    window.scrollTo({
         top: offsetPosition,
         behavior: "smooth"
    });
}