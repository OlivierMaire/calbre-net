export function onLoad() {
    console.log('Loaded');



    onUpdate();
}

export function onUpdate() {
    console.log('Updated');

    document.querySelectorAll(".mud-input input.mud-input-slot")
        .forEach((e) => {
            e.addEventListener('blur', (event) => onblur(event.target));
            // e.blur(); 
        });

    document.querySelectorAll(".mud-input-control .mud-checkbox .mud-checkbox-input")
        .forEach((e) =>
            e.addEventListener('change', (event) => checkboxonchange(event.target)));

    document.querySelectorAll("form")
        .forEach((elem) =>
            elem.addEventListener('reset', () => {
                setTimeout(function () {
                    // executes after the form has been reset
                    console.log("reset!!");
                    document.querySelectorAll(".mud-input input.mud-input-slot")
                        .forEach((e) => onblur(e));
                    document.querySelectorAll(".mud-input-control .mud-checkbox .mud-checkbox-input")
                        .forEach((e) => checkboxonchange(e));
                }, 1);

            }));

}

function onblur(e) {
    if (e.value) {
        e.parentNode.classList.add("mud-shrink");
    } else {
        e.parentNode.classList.remove("mud-shrink");
    }
}

function checkboxonchange(e) {
    if (e.checked) {
        console.log("${event.target.name} Checked");
        e.parentNode.querySelector(".mud-icon-root").innerHTML =
            '<path d="M0 0h24v24H0z" fill="none"></path><path d="M19 3H5c-1.11 0-2 .9-2 2v14c0 1.1.89 2 2 2h14c1.11 0 2-.9 2-2V5c0-1.1-.89-2-2-2zm-9 14l-5-5 1.41-1.41L10 14.17l7.59-7.59L19 8l-9 9z"></path>';
    } else {
        console.log("${event.target.name} Unchecked");
        e.parentNode.querySelector(".mud-icon-root").innerHTML =
            '<path d="M0 0h24v24H0z" fill="none"></path><path d="M19 5v14H5V5h14m0-2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2z"></path>';
    }
}

export function onDispose() {
    console.log('Disposed');
}



// </svg>


// </svg>