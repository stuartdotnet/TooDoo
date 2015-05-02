$(function () {

    // Override default US validation 
    jQuery.validator.methods["date"] = function (value, element) {
        try {
            jQuery.datepicker.parseDate('dd/mm/yy', value);
            return true;
        }
        catch (e) { return false; }
    }

    $('#title').focus();
    $('.date-input').datepicker({ dateFormat: 'dd/mm/yy' } );
    
});