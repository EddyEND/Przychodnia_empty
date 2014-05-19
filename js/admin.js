jQuery(document).ready(function () {
    
    jQuery('#delAll').on('click', function (event) {
        jQuery('input[name^="del"]').prop('checked', jQuery('#delAll').prop('checked'));
    });

    jQuery('#delete').on('click', function (event) {
        if (!confirm("Czy na pewno chcesz usunąć konta użytkowników?"))
            event.preventDefault();
    });

    jQuery('#deleteNews').on('click', function (event) {
        if (!confirm("Czy na pewno chcesz usunąć aktualności?"))
            event.preventDefault();
    });

});