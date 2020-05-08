// DataTables initialisation
window.InitDataTable = () =>
    $(document).ready(() =>
        $('#patients-table').DataTable()
    );

// DataTable cleanup
window.DestroyDataTable = () =>
    $('#patients-table').DataTable().destroy(true);

window.ForceReload = () =>
    window.location.reload(true);