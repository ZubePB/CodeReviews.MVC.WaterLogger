// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function OrderBy(order)
{
    currentOrder = document.getElementById('current-order').value;
    records = GetRecords();
    if (currentOrder == order) {
        if (order === "Type") records = records.sort((a, b) => a[1] < b[1]);
        if (order === "Date") records = records.sort((a, b) => a[2] < b[2]);
        if (order === "Reps") records = records.sort((a, b) => a[3] < b[3]);
    }
    else
    {
        if (order === "Type") records = records.sort((a, b) => a[1] > b[1]);
        if (order === "Date") records = records.sort((a, b) => a[2] > b[2]);
        if (order === "Reps") records = records.sort((a, b) => a[3] > b[3]);
    }
    tb = document.getElementById('ind-tb');
    childs = tb.children;
    for (let i = 0; i < tb.childNodes.length; i++)
    {
        tb.childNodes[i].innerHtml = `< tr class= "d-table-row index-tr" > <td class="index-data-row-1" id="type${records[i,0]}" > ${records[i,1]} </td > <td class="index-data-row-2" id="date${records[i,0]}" > ${records[i,2]} </td > <td class="index-data-row-3" id="reps${records[i,0]}" > ${records[i,3]} </td > <td class="index-data-row index-data-btns"> <a button type="button" class="btn btn-danger ex-btn" asp-page="./Delete" asp-route-id="${records[i,0]}"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16"> <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5zm3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0V6z" /> <path fill-rule="evenodd" d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1v1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4H4.118zM2.5 3V2h11v1h-11z" /> </svg> </a> <a button type="button" class="btn btn-primary ex-btn" asp-page="./Update" asp-route-id="${records[i,0]}"> <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-pen" viewBox="0 0 16 16">  <path d="m13.498.795.149-.149a1.207 1.207 0 1 1 1.707 1.708l-.149.148a1.5 1.5 0 0 1-.059 2.059L4.854 14.854a.5.5 0 0 1-.233.131l-4 1a.5.5 0 0 1-.606-.606l1-4a.5.5 0 0 1 .131-.232l9.642-9.642a.5.5 0 0 0-.642.056L6.854 4.854a.5.5 0 1 1-.708-.708L9.44.854A1.5 1.5 0 0 1 11.5.796a1.5 1.5 0 0 1 1.998-.001zm-.644.766a.5.5 0 0 0-.707 0L1.95 11.756l-.764 3.057 3.057-.764L14.44 3.854a.5.5 0 0 0 0-.708l-1.585-1.585z" />  </svg> </a> </td> </tr >`;
    }
    document.getElementById('current-order').value = order;
    currentOrder = document.getElementById('current-order').value;
    tb.innerHtml = " ";
    debugger;
}

function GetRecords() {
    ids = document.getElementsByClassName('index-data-id');
    types = document.getElementsByClassName('index-data-row-1');
    dates = document.getElementsByClassName('index-data-row-2');
    reps = document.getElementsByClassName('index-data-row-3');
    let exs = [];
    for (let i = 0; i < types.length; i++)
    {
        exs.push([ids[i],types[i].innerText, dates[i].innerText, reps[i].innerText]);
    }
    return exs;
}