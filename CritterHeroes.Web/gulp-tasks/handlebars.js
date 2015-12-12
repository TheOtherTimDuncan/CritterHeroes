'use strict';

module.exports = function (gulp, plugins, common) {

    var srcTemplates = common.srcPath + '/templates';
    var targetTemplates = common.distPath + '/templates';

    gulp.task('clean-templates', function () {
        return plugins.del([targetTemplates + '/**']);
    });

    gulp.task('app-templates', ['clean-templates'], function () {

        return gulp.src(srcTemplates + '/*.hb', { base: common.srcPath })
            .pipe(plugins.handlebars({
                handlebars: require('handlebars')
            }))
            .pipe(plugins.wrap('Handlebars.template(<%= contents %>)'))
            .pipe(plugins.declare({
                namespace: 'cheroes.templates',
                noRedeclare: false,
                root: 'global',
                processName: function (filePath) {
                    return common.path.basename(filePath, '.js').replace('ch.', '').replace('-template', '');
                }
            }))
            .pipe(plugins.wrap('(function(global){\n"use strict";\n<%= contents %>\n})(this);'))
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(plugins.rev())
            .pipe(gulp.dest(common.distPath))
            .pipe(plugins.rev.manifest("versioned-templates.json"))
            .pipe(gulp.dest('./'));

    });

    return ['clean-templates',  'app-templates'];
}
