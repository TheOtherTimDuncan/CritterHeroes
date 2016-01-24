<div class="flex-table-row flex-table-header">
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
  <div class="flex-table-row">
    <div class="flex-table-cell flex-cell-s1">
      {{#if pictureFilename}}
      <img height="50" src="{{../pictureUrl}}/{{id}}/{{pictureFilename}}" />
      {{else}}
      &nbsp;
      {{/if}}
    </div>
    <div class="flex-table-cell flex-cell-s1">{{name}}</div>
    <div class="flex-table-cell flex-cell-s1">{{sexName}}</div>
    <div class="flex-table-cell flex-cell-s1">{{status}}</div>
    <div class="flex-table-cell flex-cell-s2">{{breed}}</div>
    <div class="flex-table-cell flex-cell-s2">{{fosterName}}</div>
    <div class="flex-table-cell flex-cell-s1">
      <a href="http://www.fflah.org/animals/detail?AnimalID={{siteID}}" target="_blank">View</a>
    </div>
  </div>
  {{else}}
  <div class="flex-table-row">
    <div class="flex-table-cell flex-cell-s1">No critters found.</div>
  </div>
  {{/each}}
</div>
