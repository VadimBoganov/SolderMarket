var checkboxes = document.querySelectorAll('input[type=checkbox]');
var solders = Array.from(document.querySelectorAll('.solder'));

var checked = {};

Array.prototype.forEach.call(checkboxes, function (el) {
    el.addEventListener('change', toggleCheckbox);
});

getChecked('types');

function toggleCheckbox(e) {
    getChecked(e.target.name);
    setVisibility();
}

function getChecked(name) {
    checked[name] = Array.from(document.querySelectorAll('input[name=' + name + ']:checked')).map(function (el) {
        return el.value;
    });
}

function setVisibility() {
    solders.map(function (el) {
        var types = checked.types.length ? _.intersection(Array.from(el.classList), checked.types).length : true;
        var products = checked.products.length ? _.intersection(Array.from(el.classList), checked.products).length : true;
    
        if (types && products) {
            el.style.display = 'table-cell';
        } else {
            el.style.display = 'none';
        }
    });
}