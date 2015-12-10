{{#each critters}}
<tr>
    <td>
        {{#if pictureFilename}}
        <img height="50" src="@Url.Picture()/{{id}}/{{pictureFilename}}" />
        {{else}}
        &nbsp;
        {{/if}}
    </td>
    <td>{{name}}</td>
    <td>{{sexName}}</td>
    <td>{{status}}</td>
    <td>{{breed}}</td>
    <td>{{fosterName}}</td>
    <td><a href="http://www.fflah.org/animals/detail?AnimalID={{siteID}}" target="_blank">View on site</a></td>
</tr>
{{else}}
<tr>
    <td colspan="7">No critters found.</td>
</tr>
{{/each}}
