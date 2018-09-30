This project is designed to make the Flutter SDK available on the .NET Framework, initially with the supported platforms of:

1) Xamarin.Android
2) Xamarin.iOS
3) UWP

The is no reason this couldn't also expand to any place where [SkiaSharp](https://github.com/mono/SkiaSharp) is supported.

# Overview

To make this work, numerous components need to come together.

## Transpiler

The transpiler, will convert the existing Flutter SDK (written in Dart) to CSharp, for consumption in .NET applications.

A custom DartReader (Parser) has been created. This creates a Model, that is passed to the CSharpWriter.

The CSharpWriter uses Rosyln to output the model in C#.

## Bindings

Flutter Bindings will include the connection between SkiaSharp and the Flutter SDK. We will not be using the actual Flutter engine, though we may do in the future.

Due to previous exploratory work, the Flutter engine is difficult to integrate with since we need to expose many C++ level APIs. Due to how this is implemented in the Flutter engine currently, it would require certain modifications and would be difficult to keep in sync with the master repository.

Hence, we will map the calls directly to SkiaSharp and Harfbuzz to draw directly on a SkiaCanvas. I could regret this, we shall see :)

## Shell

The Shell, is where the Skia Canvas is initialized and platform level events are collected, and sent through to the Bindings project.

# Contributing

We welcome any help. We have a Slack Channel in [XamarinChat](https://xamarinchat.herokuapp.com/). Its private, so tweet me [@adpedley](https://twitter.com/adpedley) if you want to be added.

Also look at the Projects board to see what is available. The tasks will be managed through there. If you think you can do anything on there, comment on it, and I will assign it to you.

Please note our [Code of Conduct](CODE_OF_CONDUCT.md)

## Skills needed

1) Experience in C# is essential.

2) Experience in Dart is an advantage in dealing with the Transpiler

3) Experience in Skia/SkiaSharp is an advantage in dealing with the Flutter Bindings

4) Experience in Xamarin.Android, Xamarin.iOS and/or UWP is beneficial for working with the Shell

You can be on a Mac or Windows. This is a Visual Studio 2017 solution file and projects. Having at least the Xamarin components of Visual Studio is required when dealing with the Shell.

Only C#/.NET are used in this project. Though we do parse Dart files. No C++ or other language skills needed.

## How to contribute

1) Create a fork of this project. Then clone your fork locally.

2) Create a new branch on your fork, per card. NOTE: Do not do work directly on the master branch.

3) Once complete, submit a PR to this project repository.

4) It will be reviewed and merged once reviewed.

## Rebasing your fork

Over time, your fork will become out of date with the main repository. If you don't know how to rebase, this is a sample on how to do it through the command line.

1) Open a Command Prompt or Terminal window

### Only do these once
2) Make sure you haven't already added an upstream by typing 

   `git remote -v`

You should have something similar to 

   ```
   origin  https://github.com/YOUR_USERNAME/YOUR_FORK.git (fetch)   
   origin  https://github.com/YOUR_USERNAME/YOUR_FORK.git (push)
   ```

3) Add upstream by typing this command:

   `git remote add upstream https://github.com/adamped/xamarin.flutter.git`

### Do these every rebase
4) Ensure the master branch is checked out. 

   `git checkout master`

5) Reset the master branch. This is just to confirm nothing is lurking there.

   `git reset --hard HEAD`

6) Fetch the latest upstream

   `git fetch upstream`

7) Rebase upstream on to master

   `git rebase upstream/master`

8) Force push your new rebased master to your fork

   `git push origin master --force`

### If you need to rebase a branch as well (complete the above steps, then do these as well)
9) Checkout your branch (do not include the angle brackets with your branch name < > )

   `git checkout <branch name>`

10) Rebase your branch

    `git rebase master/<branch name>`

11) Force push your new rebased branch to your fork

    `git push origin <branch name> --force`

### Help its all a mess

If things have gone wrong and you have already had an existing branch PR'd and merged (PLEASE NOTE YOU NEED IT MERGED INTO THIS REPOSITORY). 
Then you can just delete your fork, and create a new one. Sometimes its an easier approach if things are a mess and you don't have work that needs to be saved. All your PR'd and merged contributions will still be existing and credited to you. You do not lose any contributions or credit for them, by deleting your fork. But they must be merged here first.
