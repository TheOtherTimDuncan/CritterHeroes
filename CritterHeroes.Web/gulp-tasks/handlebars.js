'use strict';

module.exports = function (gulp, plugins, common) {

    var srcTemplates = common.srcPath + '/js/templates';
    var targetTemplates = common.distPath + '/templates';
    var stagingTemplates = srcTemplates + '/staging';

    gulp.task('clean-templates', function () {
        return plugins.del([targetTemplates + '/**', './versioned-templates.json']);
    });

    gulp.task('clean-templates-staging', ['app-templates'], function () {
        return plugins.del(stagingTemplates);
    });

    gulp.task('stage-templates', ['clean-templates'], function () {

        return gulp.src(srcTemplates + '/*.hb')
            .pipe(plugins.handlebars())
            .pipe(plugins.wrap('Handlebars.template(<%= contents %>)'))
            .pipe(plugins.declare({
                namespace: 'cheroes.templates',
                noRedeclare: true,
            }))
            .pipe(plugins.rev())
            .pipe(gulp.dest(stagingTemplates))
            .pipe(plugins.rev.manifest({ path: "versioned-templates.json", merge: true }))
            .pipe(gulp.dest('./'));

    });

    gulp.task('app-templates', ['clean-templates', 'stage-templates'], function () {

        return gulp.src(stagingTemplates + '/*.js')
            .pipe(plugins.plumber({
                errorHandler: function (err) {
                    console.log(err);
                    this.emit('end');
                }
            }))
            .pipe(gulp.dest(targetTemplates))
            .pipe(plugins.uglify())
            .pipe(plugins.rename({ extname: '.min.js' }))
            .pipe(gulp.dest(targetTemplates));

    });

    return ['clean-templates', 'stage-templates', 'app-templates', 'clean-templates-staging'];
}
