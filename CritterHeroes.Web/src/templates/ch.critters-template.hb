<div class="flex-table-header">
    <div class="flex-table-cell flex-cell-s1">&nbsp;</div>
    <div class="flex-table-cell flex-cell-s1">Name</div>
    <div class="flex-table-cell flex-cell-s1">Sex</div>
    <div class="flex-table-cell flex-cell-s1">Status</div>
    <div class="flex-table-cell flex-cell-s2">Breed</div>
    <div class="flex-table-cell flex-cell-s2">Foster</div>
    <div class="flex-table-cell flex-cell-s1">&nbsp;</div>
</div>
<div class="flex-table-body">
    {{#each critters}}
    <div class="flex-table-row row-select">
        <div class="flex-table-cell flex-table-cell-pic flex-cell-s1">
            {{#if pictureFilename}}
            <img src="{{../pictureUrl}}/{{id}}/{{pictureFilename}}" />
            {{else}}
            &nbsp;
            {{/if}}
        </div>
        <div class="flex-table-cell flex-cell-s1 flex-table-cell-pic-margin"><span class="flex-table-cell-header flex-table-cell-header-md">Name</span><span>{{name}}</span></div>
        <div class="flex-table-cell flex-cell-s1 flex-table-cell-pic-margin"><span class="flex-table-cell-header flex-table-cell-header-md">Sex</span><span>{{sexName}}</span></div>
        <div class="flex-table-cell flex-cell-s1 flex-table-cell-pic-margin"><span class="flex-table-cell-header flex-table-cell-header-md">Status</span><span>{{status}}</span></div>
        <div class="flex-table-cell flex-cell-s2 flex-table-cell-pic-margin"><span class="flex-table-cell-header flex-table-cell-header-md">Breed</span><span>{{breed}}</span></div>
        <div class="flex-table-cell flex-cell-s2 flex-table-cell-pic-margin"><span class="flex-table-cell-header flex-table-cell-header-md">Foster</span><span>{{fosterName}}</span></div>
        <div class="flex-table-cell flex-cell-s1 flex-table-cell-pic-margin">
            <a href="http://www.fflah.org/animals/detail?AnimalID={{siteID}}" target="_blank">View on site</a>
        </div>
    </div>
    {{else}}
    <div class="flex-table-row">
        <div class="flex-table-cell flex-cell-s1">No critters found.</div>
    </div>
    {{/each}}
</div>
