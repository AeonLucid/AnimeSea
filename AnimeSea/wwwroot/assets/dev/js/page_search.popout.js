/**
 * Creates a pop-out of the image.
 * 
 * @param {MouseEvent} e The event.
 */
function popOut(e) {
    const image = e.target;
    const parent = e.target.parentElement;

    // Get the positions.
    const rect = image.getBoundingClientRect();
    const startHeight = image.clientHeight + 'px';
    const startTop = (rect.top + window.scrollY) + 'px';
    
    // Create a clone of the iamge.
    const popout = document.createElement('img');
    popout.src = image.src;
    popout.className = 'img-fluid img-thumbnail';

    popout.onload = () => {
        // Get the size of the image.
        const height = Math.min(window.innerHeight - 200, popout.height);

        // Calculate the start and end Y-coord of the image and window.
        const startY = rect.top + window.scrollY;
        const startViewY = window.scrollY + 56;
        const endY = rect.top + window.scrollY + height;
        const endViewY = window.scrollY + window.innerHeight;

        // Set styles.
        popout.style.position = 'absolute';
        popout.style.top = startTop;
        popout.style.left = rect.left + 'px';
        popout.style.zIndex = 1000;
        popout.style.maxHeight = startHeight;
        popout.style.transition = 'all .25s';
        document.body.appendChild(popout);

        // Wait 10ms for the transition.
        setTimeout(() => {
            // Make sure the image is in view.
            if (startY < startViewY) {
                popout.style.top = (startViewY + 10) + 'px';
            } else if (endY > endViewY) {
                popout.style.top = (endViewY - height - 20) + 'px';
            }

            popout.style.maxHeight = height + 'px';
        }, 10);

        // When the user clicks anywhere, hide the image.
        const close = () => {
            if (startY < startViewY || endY > endViewY) {
                popout.style.top = startTop;
            }

            popout.style.maxHeight = startHeight;

            setTimeout(() => {
                document.body.removeChild(popout);
                window.removeEventListener('mousedown', close);
            }, 250);
        };

        window.addEventListener('mousedown', close);
    };
}