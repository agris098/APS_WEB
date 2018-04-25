function CommentsManager() {
    var _container = $(".comments-container");
    var _commentsContainer = _container.find(".c-body");

    function init() {
        registerEvents();
        loadComments();
    }
    function loadComments() {
        var uri = uri = "http://localhost:56616/api/classifields/comments/" + classified.Id;
        $.ajax({
            method: "GET",
            url: uri,
            success: function (data) {
                $.each(data, function (i, item) {
                    addComment(item);
                });
            },
            error: function () { }
        });
    }

    function addComment(data) {
        var comment = _commentsContainer.find("[commentTemplate]").clone().removeAttr("commentTemplate");
        _commentsContainer.prepend(bindCommentValues(comment, data));
    }

    function bindCommentValues(comment, data) {
        comment.find("#c-picture-user").attr("src", "data:image/jpg;base64,"+data.UserPicture);
        comment.find("[c-user-info]").html(data.UserName);
        comment.find("[c-time]").html(data.DateTime);
        comment.find("[c-text]").html(data.Text);
        comment.attr("comment-id", data.Id);
        var l = comment.find(".c-like");
        var d = comment.find(".c-dislike");
        var liked = -1;
        var disliked = -1
        if (data.Likes !== null && data.Likes !== undefined) {
            liked = jQuery.inArray(currentUser.Id, data.Likes);
        }
        if (data.DisLikes !== null && data.DisLikes !== undefined) {
            disliked = jQuery.inArray(currentUser.Id, data.DisLikes);
        }
        var allowed = false;
        if (liked < 0 && disliked < 0) {
             allowed = true;
        }
        if (currentUser.Id === data.UserId) {
            l.addClass("disabled");
            d.addClass("disabled");
             allowed = true;
        }
        if (!allowed) {
            if (liked > -1) {
                l.addClass("selected");
                d.addClass("disabled");
            }
            if (disliked > -1) {
                l.addClass("disabled");
                d.addClass("selected");
            }
        }
        comment.find("[c-like]").html(data.Likes !== null && data.Likes !== undefined ? data.Likes.length : "0");
        comment.find("[c-dislike]").html(data.DisLikes !== null && data.DisLikes !== undefined ? data.DisLikes.length : "0");
        return comment;
    }
    function registerEvents() {
        _container.on("click", "#addButton", function () {
            var uri = uri = "http://localhost:56616/api/classifields/comments/add";

            var data = {
                ClassifiedId: classified.Id,
                Text: _container.find("#addText").val(),
                UserId: currentUser.Id
            };
            $.ajax({
                method: "POST",
                url: uri,
                data: data,
                success: function (comment) {
                    addComment(comment);
                },
                error: function () { }
            });
        });
        _container.on("click", "#clearButton", function () {
            _container.find("#addText").val("");
        });
        $(document).on("click", "[c-like-button]", function () {
            if ($(this).hasClass("selected") || $(this).hasClass("disabled"))
                return false;

            var commentId = $(this).closest(".comment").attr("comment-id"),
                uri = uri = "http://localhost:56616/api/classifields/comments/like";
            var like = $(this).hasClass("c-like");

            var data = {
                ClassifiedId: classified.Id,
                CommentId: commentId,
                UserId: currentUser.Id,
                Like: like
            };
            $.ajax({
                method: "POST",
                url: uri,
                data: data,
                success: function (data) {
                    commentLike(data, like);
                },
                error: function () { }
            });
        });
    }
    function commentLike(data, like) {
        var comment = _commentsContainer.find("[comment-id = " + data.CommentId + "]"),
            l = comment.find("[c-like]"),
            d = comment.find("[c-dislike]"),
            ll = comment.find(".c-like"),
            dd = comment.find(".c-dislike");
        if (like) {
            ll.addClass("selected");
            dd.addClass("disabled");
        } else {
            ll.addClass("disabled");
            dd.addClass("selected");
        }
       
        l.html(data.Likes);
        d.html(data.Dislikes);
    }

    return {
        init: init()
    }
}
var commentsManager = new CommentsManager();