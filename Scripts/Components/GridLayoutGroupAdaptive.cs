using UnityEngine;
using UnityEngine.UI;

namespace TinyUtilities.Components {
    [AddComponentMenu("Layout/Grid Layout Group Adaptive", 152)]
    public class GridLayoutGroupAdaptive : LayoutGroup {
        public enum Corner {
            UpperLeft = 0,
            UpperRight = 1,
            LowerLeft = 2,
            LowerRight = 3
        }
        
        public enum Axis {
            Horizontal = 0,
            Vertical = 1
        }
        
        public enum Constraint {
            Flexible = 0,
            FixedColumnCount = 1,
            FixedRowCount = 2
        }
        
        public Vector2 cellSize {
            get => m_CellSize;
            set => SetProperty(ref m_CellSize, value);
        }
        
        public Vector2 spacing {
            get => m_Spacing;
            set => SetProperty(ref m_Spacing, value);
        }
        
        public Corner startCorner {
            get => m_StartCorner;
            set => SetProperty(ref m_StartCorner, value);
        }
        
        public Axis startAxis {
            get => m_StartAxis;
            set => SetProperty(ref m_StartAxis, value);
        }
        
        public Constraint constraint {
            get => m_Constraint;
            set => SetProperty(ref m_Constraint, value);
        }
        
        public int constraintCount {
            get => m_ConstraintCount;
            set => SetProperty(ref m_ConstraintCount, Mathf.Max(1, value));
        }
        
        [SerializeField]
        protected Vector2 m_CellSize = new Vector2(100, 100);
        
        [SerializeField]
        protected Vector2 m_Spacing = Vector2.zero;
        
        [SerializeField]
        protected Corner m_StartCorner = Corner.UpperLeft;
        
        [SerializeField]
        protected Axis m_StartAxis = Axis.Horizontal;
        
        [SerializeField]
        protected bool m_CenterAdaptation = false;
        
        [SerializeField]
        protected Constraint m_Constraint = Constraint.Flexible;
        
        [SerializeField]
        protected int m_ConstraintCount = 2;
        
        protected GridLayoutGroupAdaptive() { }
        
        #if UNITY_EDITOR
        protected override void OnValidate() {
            base.OnValidate();
            constraintCount = constraintCount;
        }
        
        #endif
        
        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            
            int minColumns = 0;
            int preferredColumns = 0;
            
            if (m_Constraint == Constraint.FixedColumnCount) {
                minColumns = preferredColumns = m_ConstraintCount;
            } else if (m_Constraint == Constraint.FixedRowCount) {
                minColumns = preferredColumns = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
            } else {
                minColumns = 1;
                preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
            }
            
            SetLayoutInputForAxis(padding.horizontal + (cellSize.x + spacing.x) * minColumns - spacing.x,
                                  padding.horizontal + (cellSize.x + spacing.x) * preferredColumns - spacing.x,
                                  -1,
                                  0
            );
        }
        
        public override void CalculateLayoutInputVertical() {
            int minRows = 0;
            
            if (m_Constraint == Constraint.FixedColumnCount) {
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
            } else if (m_Constraint == Constraint.FixedRowCount) {
                minRows = m_ConstraintCount;
            } else {
                float width = rectTransform.rect.width;
                int cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
                minRows = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX);
            }
            
            float minSpace = padding.vertical + (cellSize.y + spacing.y) * minRows - spacing.y;
            SetLayoutInputForAxis(minSpace, minSpace, -1, 1);
        }
        
        public override void SetLayoutHorizontal() {
            SetCellsAlongAxis(0);
        }
        
        public override void SetLayoutVertical() {
            SetCellsAlongAxis(1);
        }
        
        private void SetCellsAlongAxis(int axis) {
            // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
            // and only vertical values when invoked for the vertical axis.
            // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
            // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
            // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.
            var rectChildrenCount = rectChildren.Count;
            
            if (rectChildrenCount == 0) {
                return;
            }
            
            if (axis == 0) {
                // Only set the sizes when invoked for horizontal axis, not the positions.
                
                for (int i = 0; i < rectChildrenCount; i++) {
                    RectTransform rect = rectChildren[i];
                    
                    m_Tracker.Add(this, rect, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.SizeDelta);
                    
                    rect.anchorMin = Vector2.up;
                    rect.anchorMax = Vector2.up;
                    rect.sizeDelta = cellSize;
                }
                
                return;
            }
            
            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;
            
            int cellCountX = 1;
            int cellCountY = 1;
            
            if (m_Constraint == Constraint.FixedColumnCount) {
                cellCountX = m_ConstraintCount;
                
                if (rectChildrenCount > cellCountX) cellCountY = rectChildrenCount / cellCountX + (rectChildrenCount % cellCountX > 0 ? 1 : 0);
            } else if (m_Constraint == Constraint.FixedRowCount) {
                cellCountY = m_ConstraintCount;
                
                if (rectChildrenCount > cellCountY) cellCountX = rectChildrenCount / cellCountY + (rectChildrenCount % cellCountY > 0 ? 1 : 0);
            } else {
                if (cellSize.x + spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
                
                if (cellSize.y + spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }
            
            int cornerX = (int)startCorner % 2;
            int cornerY = (int)startCorner / 2;
            
            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            
            if (startAxis == Axis.Horizontal) {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildrenCount);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildrenCount / (float)cellsPerMainAxis));
            } else {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildrenCount);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectChildrenCount / (float)cellsPerMainAxis));
            }
            
            Vector2 requiredSpace = new Vector2(actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                                                actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
            );
            
            Vector2 startOffset = new Vector2(GetStartOffset(0, requiredSpace.x), GetStartOffset(1, requiredSpace.y));
            
            int filledCellsCount = rectChildrenCount;
            
            int lastLine;
            
            if (m_CenterAdaptation) {
                if (startAxis == Axis.Horizontal) {
                    if (actualCellCountX == 1) {
                        lastLine = 0;
                    } else {
                        lastLine = rectChildrenCount % actualCellCountX;
                    }
                } else {
                    if (actualCellCountY == 1) {
                        lastLine = 0;
                    } else {
                        lastLine = rectChildrenCount % actualCellCountY;
                    }
                }
            } else {
                lastLine = 0;
            }
            
            filledCellsCount -= lastLine;
            
            for (int i = 0; i < filledCellsCount; i++) {
                int positionX;
                int positionY;
                
                if (startAxis == Axis.Horizontal) {
                    positionX = i % cellsPerMainAxis;
                    positionY = i / cellsPerMainAxis;
                } else {
                    positionX = i / cellsPerMainAxis;
                    positionY = i % cellsPerMainAxis;
                }
                
                if (cornerX == 1) {
                    positionX = actualCellCountX - 1 - positionX;
                }
                
                if (cornerY == 1) {
                    positionY = actualCellCountY - 1 - positionY;
                }
                
                SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);
                SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY, cellSize[1]);
            }
            
            float offset;
            
            if (startAxis == Axis.Horizontal) {
                float emptyCellsCount;
                
                if (constraint == Constraint.FixedColumnCount && actualCellCountX == lastLine) {
                    emptyCellsCount = constraintCount - lastLine;
                } else {
                    emptyCellsCount = actualCellCountX - lastLine;
                }
                
                offset = (cellSize[0] + spacing[0]) * 0.5f;
                offset *= emptyCellsCount;
            } else {
                float emptyCellsCount;
                
                if (constraint == Constraint.FixedRowCount && actualCellCountY == lastLine) {
                    emptyCellsCount = constraintCount - lastLine;
                } else {
                    emptyCellsCount = actualCellCountY - lastLine;
                }
                
                offset = (cellSize[1] + spacing[1]) * 0.5f;
                offset *= emptyCellsCount;
            }
            
            for (int i = filledCellsCount; i < rectChildrenCount; i++) {
                int positionX;
                int positionY;
                
                if (startAxis == Axis.Horizontal) {
                    positionX = i % cellsPerMainAxis;
                    positionY = i / cellsPerMainAxis;
                } else {
                    positionX = i / cellsPerMainAxis;
                    positionY = i % cellsPerMainAxis;
                }
                
                if (cornerX == 1) {
                    positionX = actualCellCountX - 1 - positionX;
                }
                
                if (cornerY == 1) {
                    positionY = actualCellCountY - 1 - positionY;
                }
                
                float xEndPosition = startOffset.x + (cellSize[0] + spacing[0]) * positionX;
                float yEndPosition = startOffset.y + (cellSize[1] + spacing[1]) * positionY;
                
                if (startAxis == Axis.Horizontal) {
                    if (startCorner is Corner.LowerLeft or Corner.UpperLeft) {
                        xEndPosition += offset;
                    } else {
                        xEndPosition -= offset;
                    }
                } else {
                    if (startCorner is Corner.UpperLeft or Corner.UpperRight) {
                        yEndPosition += offset;
                    } else {
                        yEndPosition -= offset;
                    }
                }
                
                SetChildAlongAxis(rectChildren[i], 0, xEndPosition, cellSize[0]);
                SetChildAlongAxis(rectChildren[i], 1, yEndPosition, cellSize[1]);
            }
        }
    }
}