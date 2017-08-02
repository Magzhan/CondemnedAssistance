$(document).ready(function ($) {
    if ($("#activate-script-for-drop-down").length > 0) {
        $(document).on("change", "select", LoadDropDownList);
    }
});

function LoadDropDownList() {
    var id = $(this).prop("id");
    var addressId = 0;
    switch (id) {
        case "AddressLevelThreeId":
            return;
        case "AddressLevelTwoId":
        case "AddressLevelOneId":
            addressId = $(this).val();
            break;
        default:
            return;
    }

    $(this).parent().next().find("select").load("/Address/GetAddressList?addressId=" + addressId);
}