// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace HoloToolkit.Unity.InputModule
{
    public interface IXboxControllerHandler : IGamePadHandler
    {
        void OnXboxAxisUpdate(XboxControllerEventData eventData);
    }
}
