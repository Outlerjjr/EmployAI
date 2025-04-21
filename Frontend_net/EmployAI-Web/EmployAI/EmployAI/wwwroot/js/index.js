// Select all sidebar menu links, except the logout link
const sideLinks = document.querySelectorAll('.sidebar .side-menu li a:not(.logout)');

// Add a click event listener to each menu link
sideLinks.forEach(item => {
    const li = item.parentElement;  // Get the  <li> element of the link
    item.addEventListener('click', () => {  // When the link is clicked
        // Remove the 'active' class from all menu items
        sideLinks.forEach(i => {
            i.parentElement.classList.remove('active');
        })
        // Add the 'active' class to the clicked menu item's parent <li>
        li.classList.add('active');
    })
});

// Select the menu icon and the sidebar
const sideBar = document.querySelector('.sidebar');

// Add a click event listener to the menu icon (hamburger icon)
menuBar.addEventListener('click', () => {
    // Toggle the 'close' class on the sidebar, which will show or hide it
    sideBar.classList.toggle('close');
});

// Add a resize event listener to adjust elements when the window is resized
window.addEventListener('resize', () => {
    // If the window width is less than 768px, close the sidebar
    if (window.innerWidth < 768) {
        sideBar.classList.add('close');
    } else {
        sideBar.classList.remove('close');
    }
});
