function GetBaseUrl() {
    let origin = window.location.origin;
    let pathSegments = window.location.pathname.split('/').filter(Boolean);
    let csoIndex = pathSegments.findIndex(segment => segment.toLowerCase() === 'cso');
    let baseUrl;

    if (csoIndex !== -1) {
        let pathUpToCso = pathSegments.slice(0, csoIndex + 1).join('/');
        baseUrl = `${origin}/${pathUpToCso}`;
    } else {
        baseUrl = origin;
    }

    return baseUrl;
}

function isEmail(email) {
    var regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}
