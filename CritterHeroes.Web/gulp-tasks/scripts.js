'use strict';

module.exports = function (gulp, plugins, common) {

    var distScripts = common.distPath + '/js';
    var srcScripts = common.srcPath + '/lib';

    var bowerBase = 'bower_components';

    var hashes = {};

    var getHashes = function () {

        var collect = function (file, enc, cb) {
            if (file.revHash) {
                hashes[common.path.basename(file.revOrigPath, common.path.extname(file.revOrigPath))] = file.revHash;
            }
            this.push(file);
            return cb();
        };

        return plugins.through2.obj(collect);
    };

    gulp.task('clean-scripts', function () {
        return plugins.del([distScripts + '/**', '!' + common.distPath, srcScripts, '!' + common.srcPath]);
    });

    gulp.task('copy-scripts-src', ['clean-scripts'], function () {

        var sources = [
            bowerBase + '/jquery/dist/jquery.js',
            bowerBase + '/jquery/dist/jquery.min.js',
            bowerBase + '/jquery/dist/jquery.min.map',
            bowerBase + '/jquery-validation/dist/jquery.validate.js',
            bowerBase + '/jquery-validation/dist/jquery.validate.min.js',
            bowerBase + '/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.js',
            bowerBase + '/jquery-ajax-unobtrusive/jquery.unobtrusive-ajax.min.js',
            bowerBase + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js',
            bowerBase + '/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js'
        ];

        return gulp.src(sources, { base: bowerBase })
            .pipe(plugins.flatten())
            .pipe(gulp.dest(srcScripts));

    });

    gulp.task('copy-scripts-dist', ['clean-scripts', 'copy-scripts-src'], function () {

        return gulp.src([srcScripts + '/*.js', '!' + srcScripts + '/*.min.js'])
            .pipe(plugins.flatten())
            .pipe(plugins.rev())
            .pipe(getHashes())
            .pipe(gulp.dest(distScripts))
            .pipe(plugins.rev.manifest({ path: "versioned-lib.json" }))
            .pipe(gulp.dest('./'));

    });

    gulp.task('copy-scripts-dist-min', ['clean-scripts', 'copy-scripts-src', 'copy-scripts-dist'], function () {

        return gulp.src([srcScripts + '/*.min.js'])
            .pipe(plugins.rename(function (path) {
                var fileKey = common.path.basename(path.basename, '.min');
                path.basename = fileKey + '-' + hashes[fileKey] + '.min';
            }))
            .pipe(plugins.flatten())
            .pipe(gulp.dest(distScripts));

    });

    return ['clean-scripts', 'copy-scripts-src', 'copy-scripts-dist', 'copy-scripts-dist-min'];

}
