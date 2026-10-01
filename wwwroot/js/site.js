// Add / remove subject rows on the marks form and stop the same subject being picked twice.
(function () {
    var list = document.getElementById('mark-list');
    if (!list) return;

    var template = document.getElementById('row-template').innerHTML;
    var addButton = document.getElementById('add-row');
    var MAX_ROWS = 10;

    function rows() { return Array.prototype.slice.call(list.querySelectorAll('.mark-row')); }

    function refresh() {
        var chosen = [];
        rows().forEach(function (row, i) {
            // Keep indexes 0,1,2... so ASP.NET binds the list correctly.
            row.querySelectorAll('[name]').forEach(function (el) {
                el.name = el.name.replace(/Marks\[[^\]]*\]/, 'Marks[' + i + ']');
            });
            var value = row.querySelector('select').value;
            if (value) chosen.push(value);
        });
        rows().forEach(function (row) {
            var select = row.querySelector('select');
            select.querySelectorAll('option').forEach(function (o) {
                o.disabled = o.value !== '' && o.value !== select.value && chosen.indexOf(o.value) !== -1;
            });
        });
        addButton.disabled = rows().length >= MAX_ROWS;
    }

    list.addEventListener('change', function (e) { if (e.target.matches('select')) refresh(); });

    list.addEventListener('click', function (e) {
        var remove = e.target.closest('.remove');
        if (!remove) return;
        remove.closest('.mark-row').remove();
        refresh();
    });

    addButton.addEventListener('click', function () {
        list.insertAdjacentHTML('beforeend', template.replace(/__i__/g, rows().length));
        refresh();
    });

    refresh();
})();
